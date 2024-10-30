namespace GrainElevatorAPI.DTO.DTOs;

public class ProductionBatchDto
{
	public int Id { get; set; }

	public int? Waste { get; set; }

	public int? Shrinkage { get; set; }

	public int? AccountWeight { get; set; }
	
	public double? QuantitiesDrying { get; set; }
	

	public int LaboratoryCardId { get; set; }

	public int? RegisterId { get; set; }
	

	public DateTime CreatedAt { get; set; }

	public DateTime? ModifiedAt { get; set; }

	public DateTime? RemovedAt { get; set; }

	public DateTime? RestoredAt { get; set; }
	

	public int CreatedById { get; set; }

	public int? ModifiedById { get; set; }

	public int? RemovedById { get; set; }

	public int? RestoreById { get; set; }
}
