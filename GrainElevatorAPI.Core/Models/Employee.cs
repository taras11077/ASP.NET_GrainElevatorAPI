using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class Employee : IAuditable, IEmployee
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
    
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "CreatedAt must be between 1900 and 2024.")]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ModifiedAt must be between 1900 and 2024.")]
    public DateTime? ModifiedAt { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "RemovedAt must be between 1900 and 2024.")]
    public DateTime? RemovedAt { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "RestoredAt must be between 1900 and 2024.")]
    public DateTime? RestoredAt { get; set; }

    
    [Range(1, int.MaxValue, ErrorMessage = "CreatedById must be a positive number.")]
    public int? CreatedById { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "ModifiedById must be a positive number.")]
    public int? ModifiedById { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "RemovedById must be a positive number.")]
    public int? RemovedById { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "RestoreById must be a positive number.")]
    public int? RestoreById { get; set; }
    

    public virtual Employee CreatedBy { get; set; }
    public virtual Employee? ModifiedBy { get; set; }
    public virtual Employee? RemovedBy { get; set; }
    public virtual Employee? RestoreBy { get; set; }
    
    
    
    public virtual Role Role { get; set; } 
    
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<InputInvoice> InputInvoices { get; set; } = new List<InputInvoice>();
    public virtual ICollection<LaboratoryCard> LaboratoryCards { get; set; } = new List<LaboratoryCard>();
    public virtual ICollection<ProductionBatch> ProductionBatches { get; set; } = new List<ProductionBatch>();
    public virtual ICollection<InvoiceRegister> Registers { get; set; } = new List<InvoiceRegister>();
    public virtual ICollection<CompletionReportItem> CompletionReportItems { get; set; } = new List<CompletionReportItem>();
    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<PriceListItem> PriceListItems { get; set; } = new List<PriceListItem>();
    public virtual ICollection<ProductionPriceList> PriceLists { get; set; } = new List<ProductionPriceList>();
    
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

