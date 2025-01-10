using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class EmployeeDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
    
    
    [MinLength(2, ErrorMessage = "FirstName must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "FirstName must be at least 30 characters long.")]
    public string FirstName { get; set; }
    
    
    [MinLength(2, ErrorMessage = "LastName must be at least 2 characters long.")]
    [MaxLength(30, ErrorMessage = "LastName must be at least 30 characters long.")]
    public string LastName { get; set; }
    
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2075-12-31", ErrorMessage = "BirthDate must be between 1900 and 2075.")]
    public DateTime? BirthDate { get; set; }
    
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
    
    
    [Phone(ErrorMessage = "Invalid phone number.")]
    [MaxLength(15, ErrorMessage = "Phone number can't exceed 15 characters.")]
    public string? Phone { get; set; }
    
    
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be 'Male' or 'Female'.")]
    public string? Gender { get; set; }
    
    
    [MaxLength(50, ErrorMessage = "City name can't exceed 50 characters.")]
    public string? City { get; set; }
    
    
    [MaxLength(50, ErrorMessage = "Country name can't exceed 50 characters.")]
    public string? Country { get; set; }
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2075-12-31", ErrorMessage = "LastSeenOnline must be between 1900 and 2075.")]
    public DateTime LastSeenOnline { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 6 characters long.")]
    public string PasswordHash { get; set; }
    
    [Required(ErrorMessage = "RoleTitle is required.")]
    public string RoleTitle { get; set; }
    
    public string CreatedByName { get; set; }
}