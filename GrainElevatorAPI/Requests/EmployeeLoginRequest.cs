using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Requests;

public class EmployeeLoginRequest 
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [MinLength(6, ErrorMessage = "Email must be at least 6 characters long.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }
}