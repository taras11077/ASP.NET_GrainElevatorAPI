using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Core.Models;

public class AppDefect
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Description is required.")]
    [MinLength(4, ErrorMessage = "Description must be at least 4 characters long.")]
    [MaxLength(300, ErrorMessage = "Description must be at least 300 characters long.")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "ArrivalDate is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ArrivalDate must be between 1900 and 2024.")]
    public DateTime CreatedDate { get; set; }
    
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string? CompanyName { get; set; }
    
    public bool Status { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "CreatedById must be a positive number.")]
    public int CreatedById { get; set; }
    public virtual Employee? CreatedBy { get; set; }
}

// Smtp протокол - відправити на пошту розробника


