using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Core.Models;

public class Role
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
	public string Title { get; set; }
}


// admin
// CEO
// laboratory
// technologist
// accountant
// HR