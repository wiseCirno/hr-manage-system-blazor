using System.Data;
using HRMSystemLiu.Model;
using MySqlConnector;

namespace HRMSystemLiu.DAL;

public class OperationLogService
{
    public int TotalPages { get; private set; }

    public static async Task<bool> AddOperationLogAsync(OperationLog log)
    {
        const string addOp = "INSERT INTO OperationLog VALUES (@Id, @OperatorId, @ActionDate, @ActionDesc)";
        var parameters = new[]
        {
            new MySqlParameter("@Id", log.Id),
            new MySqlParameter("@OperatorId", log.OperatorId),
            new MySqlParameter("@ActionDate", log.ActionDate),
            new MySqlParameter("@ActionDesc", log.ActionDesc)
        };
        return await SqlHelper.ExecuteNonQueryAsync(addOp, parameters) > 0;
    }

    /// <summary>
    ///     查询日志并计算分页总页数。
    /// </summary>
    /// <param name="pageNo">当前页码。pageNo 从 1 开始计数。</param>
    /// <param name="pageSize">每页的记录数。</param>
    /// <returns>分页后的日志列表数据。</returns>
    public async Task<DataTable?> GetOperationLogListAsync(int pageNo, int pageSize)
    {
        if (pageNo < 1)
            throw new ArgumentException("pageNo must be greater than or equal to 1", nameof(pageNo));

        const string countLog = "SELECT COUNT(*) FROM OperationLog";
        var countObj = await SqlHelper.ExecuteScalarAsync(countLog);
        if (countObj != null && int.TryParse(countObj.ToString(), out var totalRecords))
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        else
            TotalPages = 0;

        var offset = (pageNo - 1) * pageSize;
        var getLog = $"""
                          SELECT
                            o.ActionDate                                                             AS Date,
                            IF(o.OperatorId = {Guid.Empty} OR op.Id IS NULL, 'UNKNOWN', op.RealName) AS Operator,
                            o.ActionDesc                                                             AS Description
                          FROM OperationLog o
                          LEFT JOIN Operator op ON op.Id = o.OperatorId
                          ORDER BY o.ActionDate DESC
                          LIMIT @pageSize OFFSET @offset;
                      """;
        var parameters = new[]
        {
            new MySqlParameter("@PageSize", pageSize),
            new MySqlParameter("@Offset", offset)
        };
        return await SqlHelper.QueryDataTableAsync(getLog, parameters);
    }
}