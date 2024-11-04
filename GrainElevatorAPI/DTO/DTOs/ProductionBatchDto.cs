using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class ProductionBatchDto
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }
	
	[Range(1, int.MaxValue, ErrorMessage = "Waste must be a positive number.")]
	public int? Waste { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "Shrinkage must be a positive number.")]
	public int? Shrinkage { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "AccountWeight must be a positive number.")]
	public int? AccountWeight { get; set; }
	
	[Range(1, double.MaxValue, ErrorMessage = "QuantitiesDrying must be a positive number.")]
	public double? QuantitiesDrying { get; set; }
	
	
	[Required(ErrorMessage = "LaboratoryCardId is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "LaboratoryCardId must be a positive number.")]
	public int LaboratoryCardId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "RegisterId must be a positive number.")]
	public int? RegisterId { get; set; }
}
