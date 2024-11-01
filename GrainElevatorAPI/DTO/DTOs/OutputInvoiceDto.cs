namespace GrainElevatorAPI.DTO.DTOs;

public class OutputInvoiceDto
{
	public int Id { get; set; }

	public string InvoiceNumber { get; set; }

	public DateTime ShipmentDate { get; set; }

	public string? VehicleNumber { get; set; }

	public string ProductCategory { get; set; }

	public int ProductWeight { get; set; }

	public int SupplierId { get; set; }

	public int ProductId { get; set; }

	public int WarehouseUnitId { get; set; }
	

	public DateTime CreatedAt { get; set; }

	public DateTime? ModifiedAt { get; set; }

	public DateTime? RemovedAt { get; set; }

	public DateTime? RestoredAt { get; set; }

	public int CreatedById { get; set; }

	public int? ModifiedById { get; set; }

	public int? RemovedById { get; set; }

	public int? RestoreById { get; set; }

}