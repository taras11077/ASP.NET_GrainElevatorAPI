namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IProductionBatch
{
	int Id { get; set; }
	int? Waste { get; set; }
	int? Shrinkage { get; set; }
	int? AccountWeight { get; set; }
	double? QuantitiesDrying { get; set; }
	
	int LaboratoryCardId { get; set; }
	int? RegisterId { get; set; }
	
}