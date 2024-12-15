using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class InputInvoiceDto
{
    [Required(ErrorMessage = "ProductWeight is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int Id { get; set; }
    
    [MinLength(3, ErrorMessage = "InvoiceNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "InvoiceNumber must be at least 9 characters long.")]
    public string InvoiceNumber { get; set; }
    
    [Required(ErrorMessage = "ArrivalDate is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ArrivalDate must be between 1900 and 2024.")]
    public DateTime ArrivalDate { get; set; }
    
    [MinLength(2, ErrorMessage = "VehicleNumber must be at least 2 characters long.")]
    [MaxLength(10, ErrorMessage = "VehicleNumber must be at least 9 characters long.")]
    public string? VehicleNumber { get; set; } 
    
    [Required(ErrorMessage = "PhysicalWeight is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "PhysicalWeight must be a positive number.")]
    public int PhysicalWeight { get; set; }
    
    
    
    [Range(1, int.MaxValue, ErrorMessage = "LaboratoryCardId must be a positive number.")]
    public int? LaboratoryCardId { get; set; }
    
    [Required(ErrorMessage = "SupplierTitle is required.")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public required string SupplierTitle { get; set; }
    
    [Required(ErrorMessage = "ProductTitle is required.")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public required string ProductTitle { get; set; }
    public string CreatedByName { get; set; }
    
    public bool? IsFinalized { get; set; }
}