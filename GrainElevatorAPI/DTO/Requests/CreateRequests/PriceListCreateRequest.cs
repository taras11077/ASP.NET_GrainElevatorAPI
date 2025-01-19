using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class PriceListCreateRequest
{
    [Required(ErrorMessage = "ProductTitle is required.")]
    [MinLength(2, ErrorMessage = "ProductTitle must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "ProductTitle must be at least 30 characters long.")]
    public string ProductTitle { get; set; }
    
    //public List<PriceListItemCreateRequest> PriceListItems { get; set; } = new List<PriceListItemCreateRequest>();
}