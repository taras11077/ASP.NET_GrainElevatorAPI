using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class CompletionReportOperationDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }
    
    public string TechnologicalOperationTitle { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
    public double Amount { get; set; }
	
    public decimal? OperationCost  {get; set; }
}