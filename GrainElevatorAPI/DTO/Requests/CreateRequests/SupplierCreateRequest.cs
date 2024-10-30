using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class SupplierCreateRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(30, ErrorMessage = "Title must be at least 30 characters long.")]
    public string Title { get; set; }
}