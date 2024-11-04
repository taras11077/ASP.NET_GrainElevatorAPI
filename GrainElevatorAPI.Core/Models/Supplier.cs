using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class Supplier : AuditableEntity, ISupplier
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
    
    
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string Title { get; set; }
    
    
    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<WarehouseUnit> WarehouseUnits { get; set; } = new List<WarehouseUnit>();
    public virtual ICollection<InputInvoice> InputInvoices { get; set; } = new List<InputInvoice>();
    public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();
    public virtual ICollection<InvoiceRegister> Registers { get; set; } = new List<InvoiceRegister>();
    
}

