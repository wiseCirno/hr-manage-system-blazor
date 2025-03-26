namespace HRMSystemLiu.Model.Database;

public class Operator
{
    public Guid Id { get; init; }
    public string? UserName { get; set; }
    public string? Password { get; init; }
    public bool IsDeleted { get; init; }
    public string? RealName { get; set; }
    public bool IsLocked { get; init; }
    public bool IsAdmin { get; init; }
}