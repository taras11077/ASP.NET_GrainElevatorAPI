using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class LaboratoryCardDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }

    [MinLength(3, ErrorMessage = "LaboratoryCard Number must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "LaboratoryCard Number must be at least 9 characters long.")]
    public string LabCardNumber { get; set; }

    [Required(ErrorMessage = "WeedImpurity is required.")]
    [Range(0.0, 100.0, ErrorMessage = "WeedImpurity must be between 0.0 and 100.0")]
    public double WeedImpurity { get; set; }

    [Required(ErrorMessage = "Moisture is required.")]
    [Range(0.0, 100.0, ErrorMessage = "Moisture must be between 0.0 and 100.0")]
    public double Moisture { get; set; }

    [Range(0.0, 100.0, ErrorMessage = "GrainImpurity must be between 0.0 and 100.0")]
    public double? GrainImpurity { get; set; }

    [MaxLength(300, ErrorMessage = "SpecialNotes must be at least 300 characters long.")]
    public string? SpecialNotes { get; set; }
	
    public bool? IsProduction { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "InputInvoiceId must be a positive number.")]
    public int? ProductionBatchId { get; set; }
    
    public string CreatedByName { get; set; }
    
    public bool? IsFinalized { get; set; }
    
    
    [MinLength(3, ErrorMessage = "InvoiceNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "InvoiceNumber must be at least 9 characters long.")]
    public string InvoiceNumber { get; set; }
    
    [Required(ErrorMessage = "ArrivalDate is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ArrivalDate must be between 1900 and 2024.")]
    public DateTime ArrivalDate { get; set; }
    
    [Required(ErrorMessage = "PhysicalWeight is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "PhysicalWeight must be a positive number.")]
    public int PhysicalWeight { get; set; }
    
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string SupplierTitle { get; set; }
    
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string ProductTitle { get; set; }
}