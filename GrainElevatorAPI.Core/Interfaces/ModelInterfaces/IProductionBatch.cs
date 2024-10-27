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
	
	DateTime CreatedAt { get; set; }
	DateTime? ModifiedAt { get; set; }
	DateTime? RemovedAt { get; set; }
	DateTime? RestoredAt { get; set; }
	
	int CreatedById { get; set; }
	int? ModifiedById { get; set; }
	int? RemovedById { get; set; }
	int? RestoreById { get; set; }

}