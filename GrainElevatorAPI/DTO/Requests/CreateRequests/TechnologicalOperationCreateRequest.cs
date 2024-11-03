using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class TechnologicalOperationCreateRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(100, ErrorMessage = "Title must be at least 100 characters long.")]
    public string Title { get; set; }
}