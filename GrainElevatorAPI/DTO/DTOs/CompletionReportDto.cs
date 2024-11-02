using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class CompletionReportDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }

    [MinLength(3, ErrorMessage = "ReportNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "ReportNumber must be at least 9 characters long.")]
    public string ReportNumber { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ReportDate must be between 1900 and 2024.")]
    public DateTime ReportDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "QuantitiesDrying must be a positive number.")]
    public double? QuantitiesDrying { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "ReportPhysicalWeight must be a positive number.")]
    public double? ReportPhysicalWeight { get; set; }
	
    public bool? IsFinalized { get; set; }
    
    public List<InvoiceRegisterDto> Registers { get; set; } = new List<InvoiceRegisterDto>();
    public List<CompletionReportOperationDto> CompletionReportOperations { get; set; } = new List<CompletionReportOperationDto>();

    
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int SupplierId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PriceListId must be a positive number.")]
    public int? PriceListId { get; set; }
}