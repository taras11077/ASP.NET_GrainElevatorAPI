using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class WarehouseProductCategoryUpdateRequest
{
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string? Title { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int? Value { get; set; }
}