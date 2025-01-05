using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class InputInvoiceUpdateRequest
{
    [MinLength(3, ErrorMessage = "InvoiceNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "InvoiceNumber must be at least 9 characters long.")]
    public string? InvoiceNumber { get; set; }
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "01-01-1900", "31-12-2075", ErrorMessage = "ArrivalDate must be between 1900 and 2075.")]
    public DateTime? ArrivalDate { get; set; }
    

    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int? SupplierId { get; set; }
    

    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int? ProductId { get; set; }


    [MinLength(2, ErrorMessage = "VehicleNumber must be at least 3 characters long.")]
    [MaxLength(10, ErrorMessage = "VehicleNumber must be at least 9 characters long.")]
    public string? VehicleNumber { get; set; } 
    

    [Range(0, int.MaxValue, ErrorMessage = "PhysicalWeight must be a positive number.")]
    public int? PhysicalWeight { get; set; }
}