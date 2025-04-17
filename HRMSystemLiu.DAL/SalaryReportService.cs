using System.Data;
using HRMSystemLiu.Model.Database;
using MySqlConnector;

namespace HRMSystemLiu.DAL;

public class SalaryReportService
{
    public static async Task<List<SalaryReportItem>> GetSalaryReportItems(Guid sheetId)
    {
        const string sql = "SELECT * FROM SalaryReport WHERE SheetId = @SheetId";
        var table = await SqlHelper.QueryDataTableAsync(sql, new MySqlParameter("@SheetId", sheetId));

        return (from DataRow dr in table.Rows select MapToSalaryReportItem(dr)).ToList();
    }

    private static SalaryReportItem MapToSalaryReportItem(DataRow dr)
    {
        var reportItem = new SalaryReportItem
        {
            SerialNumber = int.TryParse(dr["序号"].ToString(), out var serial) ? serial : 0,
            Name = dr["姓名"].ToString() ?? string.Empty,
            BaseSalary = decimal.TryParse(dr["基本工资"].ToString(), out var baseSalary) ? baseSalary : 0m,
            Bonus = decimal.TryParse(dr["奖金"].ToString(), out var bonus) ? bonus : 0m,
            Fine = decimal.TryParse(dr["罚款"].ToString(), out var fine) ? fine : 0m,
            Other = decimal.TryParse(dr["其他"].ToString(), out var other) ? other : 0m,
            Total = decimal.TryParse(dr["合计"].ToString(), out var total) ? total : 0m
        };

        return reportItem;
    }
}