namespace HRMSystemLiu.Model.Database;

public class Employee
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid MarriageId { get; set; } = Guid.Empty;
    public Guid PartyId { get; set; } = Guid.Empty;
    public Guid EducationId { get; set; } = Guid.Empty;
    public Guid GenderId { get; set; } = Guid.Empty;
    
    public Guid DepartmentId { get; set; }
    public DateTime Birthday { get; set; } = DateTime.Today;
    public DateTime HireDate { get; init; } = DateTime.Today;
    public byte[]? Photo { get; set; }
    
    public string WorkNo { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public string Resume { get; set; } = string.Empty;
    public string Nation { get; set; } = string.Empty;
    public string NativePlace { get; set; } = string.Empty;

    public bool IsValid()
    {
        return new[] { Address, Email, Name, Nation, NativePlace, WorkNo, Telephone, Resume }
                   .All(s => !string.IsNullOrEmpty(s))
               && DepartmentId != Guid.Empty && EducationId != Guid.Empty && GenderId != Guid.Empty
               && MarriageId != Guid.Empty && PartyId != Guid.Empty && Birthday != DateTime.Today;
    }
}

public class EmployeeLite
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WorkNo { get; set; } = string.Empty;
}