using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class PriceListItemDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
	
    [Range(0, double.MaxValue, ErrorMessage = "OperationPrice must be a positive number.")]
    public decimal OperationPrice { get; set; }
	
    [Range(1, int.MaxValue, ErrorMessage = "PriceListId must be a positive number.")]
    public int TechnologicalOperationId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "PriceListId must be a positive number.")]
    public int PriceListId { get; set; }
}