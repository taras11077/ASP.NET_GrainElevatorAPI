using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class EmployeeUpdateRequest
{
    [MinLength(4, ErrorMessage = "FirstName must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "FirstName must be at least 20 characters long.")]
    public string? FirstName { get; set; }
    
    
    [MinLength(4, ErrorMessage = "LastName must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "LastName must be at least 20 characters long.")]
    public string? LastName { get; set; }
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2075-12-31", ErrorMessage = "Birth date must be between 1900 and 2075.")]
    public DateTime? BirthDate { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [MinLength(6, ErrorMessage = "Email must be at least 6 characters long.")]
    public string? Email { get; set; }
    
    [RegularExpression(@"^\+\d{1,3}\s?\d{7,15}$", ErrorMessage = "Phone number must be in the format +123456789 or +12 3456789.")]
    public string? Phone { get; set; }
    
    public string? Gender { get; set; }
    
    public string? City { get; set; }
    
    [MaxLength(50, ErrorMessage = "Country name can't exceed 50 characters.")]
    public string? Country { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive number.")]
    public int? RoleId { get; set; }
    
    public string? PasswordHash { get; set; }
}