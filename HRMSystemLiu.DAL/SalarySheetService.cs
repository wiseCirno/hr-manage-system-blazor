using System.Data;
using HRMSystemLiu.Model.Database;
using MySqlConnector;

namespace HRMSystemLiu.DAL;

public static class SalarySheetService
{
    public static async Task<(List<SalarySheetItem> items, List<Employee> employees)> CreateNewSalarySheetAsync(
        SalarySheet salarySheet)
    {
        const string sql = """
                           INSERT INTO SalarySheet
                           VALUES (@Id, @Year, @Month, @DepartmentId);
                           """;
        var parameters = new[]
        {
            new MySqlParameter("@Id", salarySheet.Id),
            new MySqlParameter("@Year", salarySheet.Year),
            new MySqlParameter("@Month", salarySheet.Month),
            new MySqlParameter("@DepartmentId", salarySheet.DepartmentId)
        };
        await SqlHelper.ExecuteNonQueryAsync(sql, parameters);

        var employees = await EmployeeService.GetEmployeesByDepartmentIdAsync(salarySheet.DepartmentId);
        var items = employees
            .Select(employee => new SalarySheetItem { SheetId = salarySheet.Id, EmployeeId = employee.Id })
            .ToList();

        return (items, employees);
    }

    public static async Task<(List<SalarySheetItem> items, List<Employee> employees)> RecreateSalarySheetAsync(
        SalarySheet salarySheet)
    {
        await DeleteExistedSalarySheetAndSalarySheetItemsAsync(salarySheet);
        return await CreateNewSalarySheetAsync(salarySheet);
    }

    public static async Task<(List<SalarySheetItem> items, List<Employee> employees)> GetExistedSalarySheetAsync(
        SalarySheet salarySheet)
    {
        var sheet = await GetSalarySheetAsync(salarySheet);
        var items = await GetSalarySheetItemsBySheetIdAsync(sheet.Id);
        var employees = await EmployeeService.GetEmployeesByDepartmentIdAsync(sheet.DepartmentId);
        return (items, employees);
    }

    public static async Task<SalarySheet> GetSalarySheetAsync(SalarySheet salarySheet)
    {
        if (!await IsExistSalarySheetAsync(salarySheet)) return new SalarySheet();

        const string sql =
            "SELECT * FROM SalarySheet WHERE Year = @Year AND Month = @Month AND DepartmentId = @DepartmentId;";
        var table = await SqlHelper.QueryDataTableAsync(sql, GetYearMonthDepartmentIdParameters(salarySheet));

        return MapToSalarySheet(table.Rows[0]);
    }

    public static async Task<List<SalarySheet>> GetSalarySheetListAsync()
    {
        const string sql = "SELECT * FROM SalarySheet";
        var table = await SqlHelper.QueryDataTableAsync(sql);
        return (from DataRow item in table.Rows select MapToSalarySheet(item)).ToList();
    }

    public static async Task<bool> IsExistSalarySheetAsync(SalarySheet salarySheet)
    {
        const string sql = """
                           SELECT COUNT(*)
                           FROM SalarySheet
                           WHERE Year = @Year AND Month = @Month AND DepartmentId = @DepartmentId;
                           """;

        var result = await SqlHelper.ExecuteScalarAsync(sql, GetYearMonthDepartmentIdParameters(salarySheet));
        var count = 0;
        if (result != null && result != DBNull.Value)
            count = Convert.ToInt32(result);
        return count > 0;
    }

    public static async Task UpdateSalarySheetItemsAsync(List<SalarySheetItem> items)
    {
        const string insertSql = """
                                 INSERT INTO SalarySheetItem (Id, SheetId, EmployeeId, BaseSalary, Bonus, Fine, Other)
                                 VALUES (@Id, @SheetId, @EmployeeId, @BaseSalary, @Bonus, @Fine, @Other);
                                 """;

        const string updateSql = """
                                 UPDATE SalarySheetItem
                                 SET SheetId = @SheetId,
                                     EmployeeId = @EmployeeId,
                                     BaseSalary = @BaseSalary,
                                     Bonus = @Bonus,
                                     Fine = @Fine,
                                     Other = @Other
                                 WHERE Id = @Id;
                                 """;

        foreach (var item in items)
        {
            const string checkSql = "SELECT COUNT(*) FROM SalarySheetItem WHERE Id = @Id;";
            var existingCount = await SqlHelper.ExecuteScalarAsync(checkSql, new MySqlParameter("@Id", item.Id));
            var parameters = new MySqlParameter[]
            {
                new("@Id", item.Id),
                new("@SheetId", item.SheetId),
                new("@EmployeeId", item.EmployeeId),
                new("@BaseSalary", item.BaseSalary),
                new("@Bonus", item.Bonus),
                new("@Fine", item.Fine),
                new("@Other", item.Other)
            };
            await SqlHelper.ExecuteNonQueryAsync((long)existingCount! > 0 ? updateSql : insertSql, parameters);
        }
    }

    private static async Task DeleteExistedSalarySheetAndSalarySheetItemsAsync(SalarySheet salarySheet)
    {
        const string sqlSheetItem = "DELETE FROM SalarySheetItem WHERE SheetId = @SheetId;";
        const string sqlSheet = "DELETE FROM SalarySheet WHERE Id = @Id;";
        var sheet = await GetSalarySheetAsync(salarySheet);
        await SqlHelper.ExecuteNonQueryAsync(sqlSheetItem, new MySqlParameter("@SheetId", sheet.Id));
        await SqlHelper.ExecuteNonQueryAsync(sqlSheet, new MySqlParameter("@Id", sheet.Id));
    }

    private static async Task<List<SalarySheetItem>> GetSalarySheetItemsBySheetIdAsync(Guid id)
    {
        const string sql = "SELECT * FROM SalarySheetItem WHERE SheetId = @SheetId;";
        var table = await SqlHelper.QueryDataTableAsync(sql, new MySqlParameter("@SheetId", id));
        if (table.Rows.Count == 0) return [];
        var items = (from DataRow row in table.Rows select MapToSalarySheetItem(row)).ToList();
        return items;
    }

    private static SalarySheetItem MapToSalarySheetItem(DataRow dr)
    {
        var item = new SalarySheetItem
        {
            Id = dr["Id"] is Guid idGuid ? idGuid : Guid.Parse(dr["Id"].ToString()!),
            SheetId = dr["SheetId"] is Guid sheetGuid ? sheetGuid : Guid.Parse(dr["SheetId"].ToString()!),
            EmployeeId =
                dr["EmployeeId"] is Guid employeeGuid ? employeeGuid : Guid.Parse(dr["EmployeeId"].ToString()!),
            BaseSalary = dr["BaseSalary"] is decimal bs ? bs : decimal.Parse(dr["BaseSalary"].ToString()!),
            Bonus = dr["Bonus"] is decimal bonus ? bonus : decimal.Parse(dr["Bonus"].ToString()!),
            Fine = dr["Fine"] is decimal fine ? fine : decimal.Parse(dr["Fine"].ToString()!),
            Other = dr["Other"] is decimal other ? other : decimal.Parse(dr["Other"].ToString()!)
        };

        return item;
    }

    private static SalarySheet MapToSalarySheet(DataRow dr)
    {
        var salarySheet = new SalarySheet
        {
            Id = dr["Id"] is Guid guid ? guid : Guid.Parse(dr["Id"].ToString()!),
            Year = int.TryParse(dr["Year"].ToString(), out var year) ? year : DateTime.Now.Year,
            Month = int.TryParse(dr["Month"].ToString(), out var month) ? month : DateTime.Now.Month,
            DepartmentId = dr["DepartmentId"] is Guid deptGuid ? deptGuid : Guid.Parse(dr["DepartmentId"].ToString()!)
        };

        return salarySheet;
    }

    private static MySqlParameter[] GetYearMonthDepartmentIdParameters(SalarySheet salarySheet)
    {
        return
        [
            new MySqlParameter("@Year", salarySheet.Year),
            new MySqlParameter("@Month", salarySheet.Month),
            new MySqlParameter("@DepartmentId", salarySheet.DepartmentId)
        ];
    }
}