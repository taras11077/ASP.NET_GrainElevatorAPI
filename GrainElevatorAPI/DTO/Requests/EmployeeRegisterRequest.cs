using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTOs.Requests;

public class EmployeeRegisterRequest : EmployeeLoginRequest
{
    [Required(ErrorMessage = "FirstName is required.")]
    [MinLength(2, ErrorMessage = "FirstName must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "FirstName must be at least 30 characters long.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "LastName is required.")]
    [MinLength(2, ErrorMessage = "LastName must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "LastName must be at least 30 characters long.")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [MinLength(6, ErrorMessage = "Email must be at least 6 characters long.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "RoleId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive number.")]
    public int RoleId { get; set; }
}