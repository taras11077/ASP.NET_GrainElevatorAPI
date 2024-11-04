using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class WarehouseUnitUpdateRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int? SupplierId { get; set; }
    

    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int? ProductId { get; set; }
    
    
    public ICollection<int>? WarehouseProductCategoryIds { get; set; }
    public ICollection<int>? OutputInvoiceIds { get; set; }
}