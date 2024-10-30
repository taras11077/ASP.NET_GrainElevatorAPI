using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class WarehouseProductCategoryDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string Title { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int? Value { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int? WarehouseUnitId { get; set; }
}