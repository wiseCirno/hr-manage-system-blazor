using System.Data;
using MySqlConnector;

namespace HRMSystemLiu.DAL;

public static class SqlHelper
{
    public static async Task<MySqlDataReader> ExecuteReaderAsync(string sql, params MySqlParameter[] parameters)
    {
        var conn = await Config.ConnectAsync();
        var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);
        return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
    }

    public static async Task<int> ExecuteNonQueryAsync(string sql, params MySqlParameter[] parameters)
    {
        var conn = await Config.ConnectAsync();
        try
        {
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(parameters);
            return await cmd.ExecuteNonQueryAsync();
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    public static async Task<object?> ExecuteScalarAsync(string sql, params MySqlParameter[] parameters)
    {
        var conn = await Config.ConnectAsync();
        try
        {
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(parameters);
            return await cmd.ExecuteScalarAsync();
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    public static async Task<DataTable> QueryDataTableAsync(string sql, params MySqlParameter[] parameters)
    {
        var conn = await Config.ConnectAsync();
        try
        {
            using var sda = new MySqlDataAdapter(sql, conn);
            sda.SelectCommand!.Parameters.AddRange(parameters);
            var table = new DataTable();
            sda.Fill(table);
            return table;
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    public static async Task<Dictionary<string, Guid>> QueryStringGuidDictionaryAsync(string sql, string keyName,
        string valueName, params MySqlParameter[] parameters)
    {
        await using var reader = await ExecuteReaderAsync(sql, parameters);
        var dict = new Dictionary<string, Guid>();
        while (await reader.ReadAsync())
        {
            var key = reader[$"{keyName}"] != DBNull.Value ? reader[$"{keyName}"].ToString()! : string.Empty;
            var value = reader[$"{valueName}"] != DBNull.Value ? (Guid)reader[$"{valueName}"] : Guid.Empty;
            dict.Add(key, value);
        }

        return dict;
    }
}