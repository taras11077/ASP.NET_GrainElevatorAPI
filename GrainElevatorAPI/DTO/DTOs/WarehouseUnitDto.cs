using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class WarehouseUnitDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int SupplierId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int ProductId { get; set; }
    
    public List<int> WarehouseProductCategoryIds { get; set; } = new List<int>();
    public List<int> OutputInvoiceIds { get; set; } = new List<int>();
}
