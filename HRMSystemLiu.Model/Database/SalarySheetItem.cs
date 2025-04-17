namespace HRMSystemLiu.Model.Database;

public class SalarySheetItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SheetId { get; set; } = Guid.Empty;
    public Guid EmployeeId { get; set; } = Guid.Empty;
    public decimal BaseSalary { get; set; } = decimal.Zero;
    public decimal Bonus { get; set; } = decimal.Zero;
    public decimal Fine { get; set; } = decimal.Zero;
    public decimal Other { get; set; } = decimal.Zero;
}