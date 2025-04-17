using System.Data;
using System.Text;
using HRMSystemLiu.Model.Database;
using MySqlConnector;

namespace HRMSystemLiu.DAL;

public static class EmployeeService
{
    private const string Base = """
                                SELECT
                                    e.Number,
                                    e.Name,
                                    CONVERT(e.InDay, DATE) AS InDay,
                                    e.Nation,
                                    e.NativePlace,
                                    d.Name AS Department,
                                    dm.Name AS Marriage,
                                    dp.Name AS Party,
                                    de.Name AS Education,
                                    dg.Name AS Gender
                                FROM Employee e
                                LEFT JOIN Department d ON e.DepartmentId = d.Id
                                LEFT JOIN Dictionary dm ON e.MarriageId = dm.Id AND dm.Category = '婚姻状况'
                                LEFT JOIN Dictionary dp ON e.PartyId = dp.Id AND dp.Category = '政治面貌'
                                LEFT JOIN Dictionary de ON e.EducationId = de.Id AND de.Category = '学历'
                                LEFT JOIN Dictionary dg ON e.GenderId = dg.Id AND dg.Category = '性别'
                                """;


    private const string Lite = "SELECT Id, Name, Number FROM Employee";

    private const string Add = """
                               INSERT INTO Employee 
                               VALUES (@Id, @Number, @Name, @BirthDay, @InDay, 
                               @MarriageId, @PartyId, @EducationId, @GenderId, 
                               @DepartmentId, @Telephone, @Address, 
                               @Email, @Remarks, @Resume, @Photo, @Nation, @NativePlace);
                               """;

    private const string Delete = "DELETE FROM Employee WHERE Id = @Id;";

    public static async Task<bool> AddEmployeeAsync(Employee employee)
    {
        var parameters = new[]
        {
            new MySqlParameter("@Id", employee.Id),
            new MySqlParameter("@Number", employee.WorkNo),
            new MySqlParameter("@Name", employee.Name),
            new MySqlParameter("@BirthDay", employee.Birthday),
            new MySqlParameter("@InDay", employee.HireDate),
            new MySqlParameter("@MarriageId", employee.MarriageId),
            new MySqlParameter("@PartyId", employee.PartyId),
            new MySqlParameter("@EducationId", employee.EducationId),
            new MySqlParameter("@GenderId", employee.GenderId),
            new MySqlParameter("@DepartmentId", employee.DepartmentId),
            new MySqlParameter("@Telephone", employee.Telephone),
            new MySqlParameter("@Address", employee.Address),
            new MySqlParameter("@Email", employee.Email),
            new MySqlParameter("@Remarks", employee.Remarks),
            new MySqlParameter("@Resume", employee.Resume),
            new MySqlParameter("@Photo", employee.Photo),
            new MySqlParameter("@Nation", employee.Nation),
            new MySqlParameter("@NativePlace", employee.NativePlace)
        };
        return await SqlHelper.ExecuteNonQueryAsync(Add, parameters) > 0;
    }

    public static async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        return await SqlHelper.ExecuteNonQueryAsync(Delete, new MySqlParameter("@Id", id)) > 0;
    }

    public static async Task<DataTable> GetEmployeeAsync()
    {
        return await SqlHelper.QueryDataTableAsync(Base);
    }

    public static async Task<List<EmployeeLite>> GetEmployeeLiteAsync()
    {
        var table = await SqlHelper.QueryDataTableAsync(Lite);
        var list = new List<EmployeeLite>();

        foreach (DataRow dr in table.Rows)
        {
            var employee = new EmployeeLite
            {
                Id = dr["Id"] is Guid guid ? guid : Guid.Parse(dr["Id"].ToString()!),
                Name = dr["Name"].ToString() ?? string.Empty,
                WorkNo = dr["Number"].ToString() ?? string.Empty
            };
            list.Add(employee);
        }

        return list;
    }

    public static async Task<Employee> GetEmployeeAsync(Guid id)
    {
        const string sql = "SELECT * FROM Employee WHERE Id = @Id";
        var table = await SqlHelper.QueryDataTableAsync(sql, new MySqlParameter("@Id", id));
        if (table.Rows.Count == 0)
        {
            throw new NullReferenceException($"No Employee found for id: {id}");
        }

        return MapToEmployee(table.Rows[0]);
    }

    public static async Task<DataTable> GetEmployeeAsync(SearchCondition condition)
    {
        var sql = new StringBuilder(Base);
        sql.AppendLine("WHERE TRUE");

        var parameters = new List<MySqlParameter>();

        if (condition.SearchText != null)
        {
            sql.AppendLine(" AND e.Name LIKE @name");
            parameters.Add(new MySqlParameter("@name", $"%{condition.SearchText}%"));
        }

        if (condition.DepartmentId != Guid.Empty)
        {
            sql.AppendLine(" AND e.DepartmentId = @departmentId");
            parameters.Add(new MySqlParameter("@departmentId", condition.DepartmentId!.Value));
        }

        if (condition.FromDate != default(DateTime))
        {
            sql.AppendLine(" AND e.InDay >= @fromDate");
            parameters.Add(new MySqlParameter("@fromDate", condition.FromDate!.Value));
        }

        if (condition.ToDate != default(DateTime))
        {
            sql.AppendLine(" AND e.InDay <= @toDate");
            parameters.Add(new MySqlParameter("@toDate", condition.ToDate!.Value));
        }

        return await SqlHelper.QueryDataTableAsync(sql.ToString(), parameters.ToArray());
    }
    
    public static async Task<List<Employee>> GetEmployeesByDepartmentIdAsync(Guid departmentId)
    {
        const string sql = "SELECT * FROM Employee WHERE DepartmentId = @DepartmentId";
        var table = await SqlHelper.QueryDataTableAsync(sql, new MySqlParameter("@DepartmentId", departmentId));

        return (from DataRow dr in table.Rows select MapToEmployee(dr)).ToList();
    }
    
    public static async Task<int> GetEmployeesCountByDepartmentIdAsync(Guid departmentId)
    {
        const string sql = "SELECT COUNT(*) AS EmployeeCount FROM Employee WHERE DepartmentId = @DepartmentId";
        var table = await SqlHelper.QueryDataTableAsync(sql, new MySqlParameter("@DepartmentId", departmentId));
        if (table.Rows.Count == 0) return 0;
        
        var countValue = table.Rows[0]["EmployeeCount"];
        return int.TryParse(countValue.ToString(), out var count) ? count : 0;
    }
    
    public static async Task<bool> UpdateEmployeeAsync(Employee employee)
    {
        const string sql = """
                           UPDATE Employee
                           SET 
                               MarriageId = @MarriageId,
                               PartyId = @PartyId,
                               EducationId = @EducationId,
                               GenderId = @GenderId,
                               DepartmentId = @DepartmentId,
                               Birthday = @Birthday,
                               InDay = @HireDate,
                               Photo = @Photo,
                               Number = @WorkNo,
                               Name = @Name,
                               Telephone = @Telephone,
                               Email = @Email,
                               Address = @Address,
                               Remarks = @Remarks,
                               Resume = @Resume,
                               Nation = @Nation,
                               NativePlace = @NativePlace
                           WHERE Id = @Id;
                           """;

        var parameters = new[]
        {
            new MySqlParameter("@Id", employee.Id),
            new MySqlParameter("@MarriageId", employee.MarriageId),
            new MySqlParameter("@PartyId", employee.PartyId),
            new MySqlParameter("@EducationId", employee.EducationId),
            new MySqlParameter("@GenderId", employee.GenderId),
            new MySqlParameter("@DepartmentId", employee.DepartmentId),
            new MySqlParameter("@Birthday", employee.Birthday),
            new MySqlParameter("@HireDate", employee.HireDate),
            new MySqlParameter("@Photo", employee.Photo ?? (object)DBNull.Value),
            new MySqlParameter("@WorkNo", employee.WorkNo),
            new MySqlParameter("@Name", employee.Name),
            new MySqlParameter("@Telephone", employee.Telephone),
            new MySqlParameter("@Email", employee.Email),
            new MySqlParameter("@Address", employee.Address),
            new MySqlParameter("@Remarks", employee.Remarks),
            new MySqlParameter("@Resume", employee.Resume),
            new MySqlParameter("@Nation", employee.Nation),
            new MySqlParameter("@NativePlace", employee.NativePlace)
        };

        return await SqlHelper.ExecuteNonQueryAsync(sql, parameters) > 0;
    }
    
    private static Employee MapToEmployee(DataRow dr)
    {
        var employee = new Employee
        {
            Id = dr["Id"] is Guid guid ? guid : Guid.Parse(dr["Id"].ToString()!),
            MarriageId = dr["MarriageId"] is Guid marriageGuid ? marriageGuid : Guid.Parse(dr["MarriageId"].ToString()!),
            PartyId = dr["PartyId"] is Guid partyGuid ? partyGuid : Guid.Parse(dr["PartyId"].ToString()!),
            EducationId = dr["EducationId"] is Guid eduGuid ? eduGuid : Guid.Parse(dr["EducationId"].ToString()!),
            GenderId = dr["GenderId"] is Guid genderGuid ? genderGuid : Guid.Parse(dr["GenderId"].ToString()!),
            DepartmentId = dr["DepartmentId"] is Guid deptGuid ? deptGuid : Guid.Parse(dr["DepartmentId"].ToString()!),
            Birthday = dr["Birthday"] is DateTime birthday ? birthday : DateTime.Parse(dr["Birthday"].ToString()!),
            HireDate = dr["InDay"] is DateTime hireDate ? hireDate : DateTime.Parse(dr["InDay"].ToString()!),
            Photo = dr["Photo"] as byte[],
            WorkNo = dr["Number"].ToString() ?? string.Empty,
            Name = dr["Name"].ToString() ?? string.Empty,
            Telephone = dr["Telephone"].ToString() ?? string.Empty,
            Email = dr["Email"].ToString() ?? string.Empty,
            Address = dr["Address"].ToString() ?? string.Empty,
            Remarks = dr["Remarks"].ToString() ?? string.Empty,
            Resume = dr["Resume"].ToString() ?? string.Empty,
            Nation = dr["Nation"].ToString() ?? string.Empty,
            NativePlace = dr["NativePlace"].ToString() ?? string.Empty
        };

        return employee;
    }
}