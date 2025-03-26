namespace HRMSystemLiu.Model.Database;

public class OperationLog
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid OperatorId { get; init; }
    public DateTime ActionDate { get; } = DateTime.Now;
    public string ActionDesc { get; init; } = string.Empty;
}