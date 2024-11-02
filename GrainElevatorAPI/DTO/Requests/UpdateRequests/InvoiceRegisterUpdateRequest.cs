using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class InvoiceRegisterUpdateRequest
{
    public string? RegisterNumber { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public int? SupplierId { get; set; }
    public int? ProductId { get; set; }

    public double? WeedImpurityBase { get; set; }
    public double? MoistureBase { get; set; }
    
    
    public List<int> LaboratoryCardIds { get; set; }
}