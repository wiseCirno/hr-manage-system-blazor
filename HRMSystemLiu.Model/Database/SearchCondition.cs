namespace HRMSystemLiu.Model.Database;

public class SearchCondition
{
    public string? SearchText { get; init; }
    public Guid? DepartmentId { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}