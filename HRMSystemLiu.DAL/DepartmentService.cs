using HRMSystemLiu.Model;
using MySqlConnector;

namespace HRMSystemLiu.DAL;

public static class DepartmentService
{
    public static async Task<string[]> GetDepartmentsAsync()
    {
        const string sql = "SELECT Name FROM Department ORDER BY Name";
        var departments = new List<string>();

        await using var reader = await SqlHelper.ExecuteReaderAsync(sql);
        while (await reader.ReadAsync())
            departments.Add(reader["Name"] != DBNull.Value ? reader["Name"].ToString()! : string.Empty);

        return departments.ToArray();
    }

    public static async Task<List<Department>> GetDepartmentIdDictionaryAsync()
    {
        var dict = await SqlHelper.QueryStringGuidDictionaryAsync(
            "SELECT Name, Id FROM Department ORDER BY Name", 
            "Name", 
            "Id");
        return dict.Select(pair => new Department { Name = pair.Key, Id = pair.Value }).ToList();
    }

    public static async Task<bool?> AddDepartmentAsync(string name)
    {
        if ((await GetDepartmentsAsync()).Any(existed => name == existed)) return null;
        
        var parameters = new[]
        {
            new MySqlParameter("@Id", Guid.NewGuid()),
            new MySqlParameter("@Name", name)
        };
        return await SqlHelper.ExecuteNonQueryAsync(
            "INSERT INTO Department VALUES (@Id, @Name)", 
            parameters) > 0;
    }

    public static async Task<bool> DeleteDepartment(string[] departments)
    {
        return await SqlHelper.ExecuteNonQueryAsync(
            $"DELETE FROM Department WHERE Name IN ({string.Join(", ", departments.Select((_, index) => $"@Name{index}").ToArray())})",
            departments.Select((dept, index) => new MySqlParameter($"@Name{index}", dept)).ToArray()) > 0;
    }
}