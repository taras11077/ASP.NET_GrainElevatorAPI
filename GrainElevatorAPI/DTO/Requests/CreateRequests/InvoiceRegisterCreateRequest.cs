using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class InvoiceRegisterCreateRequest
{
    [Required(ErrorMessage = "ArrivalDate is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ArrivalDate must be between 1900 and 2024.")]
    public DateTime ArrivalDate { get; set; }
    
    [Required(ErrorMessage = "RegisterNumber is required.")]
    [MinLength(3, ErrorMessage = "RegisterNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "RegisterNumber must be at least 9 characters long.")]
    public string RegisterNumber { get; set; }
    
    
    [Required(ErrorMessage = "SupplierTitle is required.")]
    [MinLength(3, ErrorMessage = "SupplierTitle must be at least 3 characters long.")]
    [MaxLength(50, ErrorMessage = "SupplierTitle must be at least 30 characters long.")]
    public string SupplierTitle { get; set; }
    
    [Required(ErrorMessage = "ProductTitle is required.")]
    [MinLength(3, ErrorMessage = "ProductTitle must be at least 3 characters long.")]
    [MaxLength(50, ErrorMessage = "ProductTitle must be at least 30 characters long.")]
    public string ProductTitle { get; set; }
    
	
    [Required(ErrorMessage = "WeedImpurityBase is required.")]
    [Range(0.0, 100.0, ErrorMessage = "WeedImpurityBase value must be between 0.0 and 100.0")]
    public double WeedImpurityBase { get; set; }

    [Required(ErrorMessage = "MoistureBase is required.")]
    [Range(0.0, 100.0, ErrorMessage = "MoistureBase value must be between 0.0 and 100.0")]
    public double MoistureBase { get; set; }
}
