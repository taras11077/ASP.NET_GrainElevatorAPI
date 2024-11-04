using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class Employee : AuditableEntity, IEmployee
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
    
    
    [MinLength(2, ErrorMessage = "FirstName must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "FirstName must be at least 30 characters long.")]
    public string? FirstName { get; set; }
    
    
    [MinLength(2, ErrorMessage = "LastName must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "LastName must be at least 30 characters long.")]
    public string? LastName { get; set; }
    
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "BirthDate must be between 1900 and 2024.")]
    public DateTime? BirthDate { get; set; }
    
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
    
    
    [Phone(ErrorMessage = "Invalid phone number.")]
    [MaxLength(15, ErrorMessage = "Phone number can't exceed 15 characters.")]
    public string? Phone { get; set; }
    
    
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be 'Male' or 'Female'.")]
    public string? Gender { get; set; }
    
    
    [MaxLength(50, ErrorMessage = "City name can't exceed 50 characters.")]
    public string? City { get; set; }
    
    
    [MaxLength(50, ErrorMessage = "Country name can't exceed 50 characters.")]
    public string? Country { get; set; }
    
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string PasswordHash { get; set; }
    
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "LastSeenOnline must be between 1900 and 2024.")]
    public DateTime LastSeenOnline { get; set; }
    
    
    [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive number.")]
    public int RoleId { get; set; }
    
    public virtual Role Role { get; set; } 
    
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<InputInvoice> InputInvoices { get; set; } = new List<InputInvoice>();
    public virtual ICollection<LaboratoryCard> LaboratoryCards { get; set; } = new List<LaboratoryCard>();
    public virtual ICollection<ProductionBatch> ProductionBatches { get; set; } = new List<ProductionBatch>();
    public virtual ICollection<InvoiceRegister> Registers { get; set; } = new List<InvoiceRegister>();
    public virtual ICollection<CompletionReportOperation> CompletionReportItems { get; set; } = new List<CompletionReportOperation>();
    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<PriceListItem> PriceListItems { get; set; } = new List<PriceListItem>();
    public virtual ICollection<PriceList> PriceLists { get; set; } = new List<PriceList>();
    
    public virtual ICollection<WarehouseUnit> WarehouseUnits { get; set; } = new List<WarehouseUnit>();
    public virtual ICollection<WarehouseProductCategory> WarehouseProductCategories { get; set; } = new List<WarehouseProductCategory>();
    public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();
    
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

