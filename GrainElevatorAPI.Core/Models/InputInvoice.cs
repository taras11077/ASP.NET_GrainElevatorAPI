using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class InputInvoice : AuditableEntity, IInputInvoice
{
	[Required(ErrorMessage = "ProductWeight is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public int Id { get; set; }
    
    [MinLength(3, ErrorMessage = "InvoiceNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "InvoiceNumber must be at least 9 characters long.")]
    public string InvoiceNumber { get; set; }
    
    [Required(ErrorMessage = "ArrivalDate is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2075-12-31", ErrorMessage = "ArrivalDate must be between 1900 and 2075.")]
    public DateTime ArrivalDate { get; set; }
    
    [MinLength(2, ErrorMessage = "VehicleNumber must be at least 2 characters long.")]
    [MaxLength(10, ErrorMessage = "VehicleNumber must be at least 9 characters long.")]
    public string? VehicleNumber { get; set; } 
    
    [Required(ErrorMessage = "PhysicalWeight is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "PhysicalWeight must be a positive number.")]
    public int PhysicalWeight { get; set; }
    
    public bool? IsFinalized { get; set; }
    
    
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int SupplierId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int ProductId { get; set; }
    
    public virtual LaboratoryCard? LaboratoryCard { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual Product Product { get; set; }
}

