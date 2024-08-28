namespace GrainElevatorAPI.Core.Models;

public class InputInvoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime ArrivalDate { get; set; }
    public string VehicleNumber { get; set; } 
    public int PhysicalWeight { get; set; }
    
    public int? LaboratoryCardId { get; set; }
    public int SupplierId { get; set; }
    public int ProductId { get; set; }
    public int CreatedById { get; set; } 
    
    public virtual LaboratoryCard? LaboratoryCard { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual Product Product { get; set; }
    public virtual Employee CreatedBy { get; set; }
}

