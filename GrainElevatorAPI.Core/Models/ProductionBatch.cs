using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Calculators.Impl;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class ProductionBatch : AuditableEntity, IProductionBatch
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
	
	public double? QuantitiesDrying { get; set; }
	
	
	[Required(ErrorMessage = "LaboratoryCardId is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "LaboratoryCardId must be a positive number.")]
	public int LaboratoryCardId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "RegisterId must be a positive number.")]
	public int? RegisterId { get; set; }
	
	public virtual LaboratoryCard LaboratoryCard { get; set; }
	public virtual InvoiceRegister? Register { get; set; }
	
}

