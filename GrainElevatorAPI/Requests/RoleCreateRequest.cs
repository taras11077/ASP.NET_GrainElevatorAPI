using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Requests;

public class RoleCreateRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string Title { get; set; }
}