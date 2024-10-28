namespace GrainElevatorAPI.DTOs;

public class WarehouseUnitDto
{
    public int Id { get; set; }
    
    public int SupplierId { get; set; }
    public int ProductId { get; set; }
    
    public List<int> WarehouseProductCategoryIds { get; set; } = new List<int>();
    public List<int> OutputInvoiceIds { get; set; } = new List<int>();
}
