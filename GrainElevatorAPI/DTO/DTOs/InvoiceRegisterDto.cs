namespace GrainElevatorAPI.DTO.DTOs;

public class InvoiceRegisterDto
{
	public int Id { get; set; }
	
	public string RegisterNumber { get; set; }

	public DateTime ArrivalDate { get; set; }

	public double WeedImpurityBase { get; set; }

	public double MoistureBase { get; set; }
	
	public int? PhysicalWeightReg { get; set; }

	public int? ShrinkageReg { get; set; }

	public int? WasteReg { get; set; }

	public int? AccWeightReg { get; set; }

	public double? QuantitiesDryingReg { get; set; }
	
	public List<ProductionBatchDto> ProductionBatches { get; set; }
	
	public int SupplierId { get; set; }
	
	public int ProductId { get; set; }

	public int? CompletionReportId { get; set; }
	
	
	public DateTime CreatedAt { get; set; }
	
	public DateTime? ModifiedAt { get; set; }

	public DateTime? RemovedAt { get; set; }

	public DateTime? RestoredAt { get; set; }
	

	public int CreatedById { get; set; }

	public int? ModifiedById { get; set; }

	public int? RemovedById { get; set; }

	public int? RestoreById { get; set; }
}