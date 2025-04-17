namespace HRMSystemLiu.Model.Database;

public class SalarySheet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Year { get; set; } = DateTime.Now.Year;
    public int Month { get; set; } = DateTime.Now.Month;
    public Guid DepartmentId { get; set; } = Guid.Empty;
}