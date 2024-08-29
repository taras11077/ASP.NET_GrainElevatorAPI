using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Requests;

public class RoleCreateRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string Title { get; set; }
}