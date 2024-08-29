namespace GrainElevatorAPI.Core.Models;

public class InputInvoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime ArrivalDate { get; set; }
    public string? VehicleNumber { get; set; } 
    public int PhysicalWeight { get; set; }
    
    public bool? Removed { get; set; } = false;
    
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
    
    public virtual LaboratoryCard? LaboratoryCard { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual Product Product { get; set; }
    public virtual Employee CreatedBy { get; set; }
    public virtual Employee? ModifiedBy { get; set; }
    public virtual Employee? RemovedBy { get; set; }
    public virtual Employee? RestoreBy { get; set; }
}

