namespace HRMSystemLiu.Model.Database;

public class Dictionary
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
}