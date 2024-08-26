namespace GrainElevatorAPI.Core.Models;

public class Employee
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; }
    
    public DateTime LastSeenOnline { get; set; }
    
    public virtual Role Role { get; set; } = null!;
    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<InputInvoice> InputInvoices { get; set; } = new List<InputInvoice>();
    public virtual ICollection<LaboratoryCard> LaboratoryCards { get; set; } = new List<LaboratoryCard>();
    public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();
    public virtual ICollection<PriceList> PriceLists { get; set; } = new List<PriceList>();
    public virtual ICollection<Register> Registers { get; set; } = new List<Register>();
    
    public Employee(string email, string passwordHash, int roleId)
    {
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        LastSeenOnline = DateTime.UtcNow;
    }

    public Employee(string? firstName, string? lastName, string email, string? phone, string? gender, string? city, string passwordHash, int roleId, string? country = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Gender = gender;
        City = city;
        PasswordHash = passwordHash;
        RoleId = roleId;
        Country = country;
        LastSeenOnline = DateTime.UtcNow;
    }

    public Employee()
    {
    }
}

