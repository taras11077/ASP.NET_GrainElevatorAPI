using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class InvoiceRegisterCreateRequest
{
    [Required]
    public int SupplierId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public DateTime ArrivalDate { get; set; }

    [Required]
    [Range(0, 100)]
    public double WeedImpurityBase { get; set; }

    [Required]
    [Range(0, 100)]
    public double MoistureBase { get; set; }

    [Required]
    public List<int> LaboratoryCardIds { get; set; }
}
