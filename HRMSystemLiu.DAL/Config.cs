using MySqlConnector;

namespace HRMSystemLiu.DAL;

public static class Config
{
    public static async Task<MySqlConnection> ConnectAsync()
    {
        var conn = new MySqlConnection("Server=localhost;UserID=root;Password=Takina714351;Database=HRMDB");
        await conn.OpenAsync();
        return conn;
    }
}