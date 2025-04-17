namespace HRMSystemLiu.Model.Database;

public class SalaryReportItem
{
    public long SerialNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Fine { get; set; }
    public decimal Other { get; set; }
    public decimal Total { get; set; }
}
