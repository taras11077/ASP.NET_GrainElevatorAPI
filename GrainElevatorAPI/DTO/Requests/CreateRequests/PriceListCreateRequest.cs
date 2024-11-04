using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class PriceListCreateRequest
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int ProductId { get; set; }
    
    public List<PriceListItemCreateRequest> PriceListItems { get; set; } = new List<PriceListItemCreateRequest>();
}