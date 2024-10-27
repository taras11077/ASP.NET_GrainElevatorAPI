namespace GrainElevatorAPI.DTOs;

public class InputInvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime ArrivalDate { get; set; }
    public string VehicleNumber { get; set; } 
    public int PhysicalWeight { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
    public DateTime? RestoredAt { get; set; }
    
    public int? LaboratoryCardId { get; set; }
    public int SupplierId { get; set; }
    public int ProductId { get; set; }
    public int CreatedById { get; set; }
    public int? ModifiedById { get; set; }
    public int? RemovedById { get; set; }
    public int? RestoreById { get; set; }
}