namespace GrainElevatorAPI.Requests;

public class WarehouseUnitUpdateRequest
{
    public int? SupplierId { get; set; }
    public int? ProductId { get; set; }
    public ICollection<int>? WarehouseProductCategoryIds { get; set; }
    public ICollection<int>? OutputInvoiceIds { get; set; }
}