namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;
using System.ComponentModel.DataAnnotations;

public class PriceListItemUpdateRequest
{
    [Range(0, double.MaxValue, ErrorMessage = "OperationPrice must be a positive number.")]
    public decimal? OperationPrice { get; set; }
	
    [Range(1, int.MaxValue, ErrorMessage = "TechnologicalOperationId must be a positive number.")]
    public int? TechnologicalOperationId { get; set; }
}