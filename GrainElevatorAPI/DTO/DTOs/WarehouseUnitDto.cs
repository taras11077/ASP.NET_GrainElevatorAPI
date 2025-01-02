using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class WarehouseUnitDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
    
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string SupplierTitle { get; set; }
    
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string ProductTitle { get; set; }
    
    public string CreatedByName { get; set; }
    
    public List<WarehouseProductCategoryDto> ProductCategories { get; set; }
}
