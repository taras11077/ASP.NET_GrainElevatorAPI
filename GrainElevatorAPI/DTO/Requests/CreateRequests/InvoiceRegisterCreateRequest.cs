using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class InvoiceRegisterCreateRequest
{
    [Required(ErrorMessage = "RegisterNumber is required.")]
    [MinLength(3, ErrorMessage = "RegisterNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "RegisterNumber must be at least 9 characters long.")]
    public string RegisterNumber { get; set; }
    
    
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int SupplierId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int ProductId { get; set; }
    
	
    [Required(ErrorMessage = "WeedImpurityBase is required.")]
    [Range(0.0, 100.0, ErrorMessage = "WeedImpurityBase value must be between 0.0 and 100.0")]
    public double WeedImpurityBase { get; set; }

    [Required(ErrorMessage = "MoistureBase is required.")]
    [Range(0.0, 100.0, ErrorMessage = "MoistureBase value must be between 0.0 and 100.0")]
    public double MoistureBase { get; set; }

    [Required]
    public List<int> LaboratoryCardIds { get; set; }
}
