namespace GrainElevatorAPI.Requests;
using System.ComponentModel.DataAnnotations;

public class CreateEmployeeRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [MinLength(4, ErrorMessage = "Email must be at least 4 characters long.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(4, ErrorMessage = "Password must be at least 4 characters long.")]
    public string Password { get; set; }
}