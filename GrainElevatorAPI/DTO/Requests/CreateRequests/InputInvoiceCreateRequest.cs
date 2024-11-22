using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class InputInvoiceCreateRequest
{
    [Required(ErrorMessage = "InvoiceNumber is required.")]
    [MinLength(3, ErrorMessage = "InvoiceNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "InvoiceNumber must be at least 9 characters long.")]
    public string InvoiceNumber { get; set; }
    
    public DateTime ArrivalDate { get; set; }
    
    [Required(ErrorMessage = "SupplierTitle is required.")]
    [MinLength(2, ErrorMessage = "SupplierTitle must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "SupplierTitle must be at least 30 characters long.")]
    public string SupplierTitle { get; set; }
    
    [Required(ErrorMessage = "ProductTitle is required.")]
    [MinLength(2, ErrorMessage = "ProductTitle must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "ProductTitle must be at least 30 characters long.")]
    public string ProductTitle { get; set; }
    
    [Required(ErrorMessage = "VehicleNumber is required.")]
    [MinLength(2, ErrorMessage = "VehicleNumber must be at least 2 characters long.")]
    [MaxLength(10, ErrorMessage = "VehicleNumber must be at least 10 characters long.")]
    public string VehicleNumber { get; set; }
    
    [Required(ErrorMessage = "PhysicalWeight is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "PhysicalWeight must be a positive number.")]
    public int PhysicalWeight { get; set; }

}