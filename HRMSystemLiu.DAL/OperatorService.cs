using HRMSystemLiu.Model;
using HRMSystemLiu.Model.Database;
using MySqlConnector;

namespace HRMSystemLiu.DAL;

public static class OperatorService
{
    public static async Task<Operator?> GetOperatorAsync(string userName)
    {
        const string sql = "select * from Operator where UserName = @userName";
        var param = new MySqlParameter("@userName", userName);
        await using var reader = await SqlHelper.ExecuteReaderAsync(sql, param);
        if (!await reader.ReadAsync()) return null;

        var op = new Operator
        {
            Id = reader["Id"] is Guid guid ? guid : Guid.Parse(reader["Id"].ToString()!),
            UserName = userName,
            Password = reader["Password"].ToString() ?? string.Empty,
            IsDeleted = bool.TryParse(reader["IsDeleted"].ToString(), out var isDeleted) && isDeleted,
            RealName = reader["RealName"].ToString() ?? string.Empty,
            IsLocked = bool.TryParse(reader["IsLocked"].ToString(), out var isLocked) && isLocked,
            IsAdmin = bool.TryParse(reader["IsAdmin"].ToString(), out var isAdmin) && isAdmin
        };
        return op;
    }

    public static async Task<bool?> CreateOperatorAsync(string userName, string password, string realName)
    {
        const string queryExisted = "SELECT 1 FROM Operator WHERE UserName = @UserName";
        var param1 = new MySqlParameter("@UserName", userName);
        await using var reader = await SqlHelper.ExecuteReaderAsync(queryExisted, param1);
        if (await reader.ReadAsync()) return null;

        const string createNew = "INSERT INTO Operator VALUES (@Id, @UserName, @Password, 0, @RealName, 1, 0)";
        var param2 = new[]
        {
            new MySqlParameter("@Id", Guid.NewGuid()),
            new MySqlParameter("@UserName", userName),
            new MySqlParameter("@Password", CommonHelper.Md5EncryptI32(password)),
            new MySqlParameter("@RealName", realName)
        };
        return await SqlHelper.ExecuteNonQueryAsync(createNew, param2) > 0;
    }
}