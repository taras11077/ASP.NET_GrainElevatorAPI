using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class PriceListItemDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
	
    [Range(0, double.MaxValue, ErrorMessage = "OperationPrice must be a positive number.")]
    public decimal OperationPrice { get; set; }
	
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(2, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string TechnologicalOperationTitle { get; set; }
    
    public string CreatedByName { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "PriceListId must be a positive number.")]
    public int PriceListId { get; set; }
}