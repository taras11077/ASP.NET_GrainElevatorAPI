using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class OutputInvoiceCreateRequest
{
    [Required(ErrorMessage = "InvoiceNumber is required.")]
    [MinLength(3, ErrorMessage = "InvoiceNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "InvoiceNumber must be at least 9 characters long.")]
    public string InvoiceNumber { get; set; }
    
    [Required(ErrorMessage = "SupplierId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int SupplierId { get; set; }
    
    [Required(ErrorMessage = "ProductId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int ProductId { get; set; }
    
    [Required(ErrorMessage = "ProductCategory is required.")]
    [MinLength(2, ErrorMessage = "ProductCategory must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "ProductCategory must be at least 30 characters long.")]
    public string ProductCategory { get; set; }
    
    [Required(ErrorMessage = "VehicleNumber is required.")]
    [MinLength(2, ErrorMessage = "VehicleNumber must be at least 2 characters long.")]
    [MaxLength(10, ErrorMessage = "VehicleNumber must be at least 10 characters long.")]
    public string VehicleNumber { get; set; }
    
    [Required(ErrorMessage = "PhysicalWeight is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "PhysicalWeight must be a positive number.")]
    public int ProductWeight { get; set; }
}