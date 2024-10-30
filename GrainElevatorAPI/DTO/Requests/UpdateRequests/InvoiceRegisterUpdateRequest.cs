using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class InvoiceRegisterUpdateRequest
{
    public string? RegisterNumber { get; set; }

    public double? WeedImpurityBase { get; set; }

    public double? MoistureBase { get; set; }
	
    public virtual ICollection<ProductionBatch>? ProductionBatches { get; set; } = new List<ProductionBatch>();
}