using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class PriceListItemCreateRequest
{
    [Range(0, double.MaxValue, ErrorMessage = "OperationPrice must be a positive number.")]
    public decimal? OperationPrice { get; set; }
	
    [Range(1, int.MaxValue, ErrorMessage = "TechnologicalOperationId must be a positive number.")]
    public int? TechnologicalOperationId { get; set; }
}