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
	
	
	public string InvoiceNumber { get; set; }
	
	public string LabCardNumber { get; set; }
	public int PhysicalWeight { get; set; }
	public double WeedImpurity { get; set; }
	public double WeedImpurityBase { get; set; }
	public double Moisture { get; set; }
	public double MoistureBase { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "RegisterId must be a positive number.")]
	public int? RegisterId { get; set; }
}
