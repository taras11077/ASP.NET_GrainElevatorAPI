using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class PriceListUpdateRequest
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int? ProductId { get; set; }
    
    public List<int> PriceListItemIds { get; set; } = new List<int>();
}