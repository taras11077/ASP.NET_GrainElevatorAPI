namespace GrainElevatorAPI.Core.Models;

public class OutputInvoice
{
    public int Id { get; set; }

    public string OutInvNumber { get; set; }
    public DateTime ShipmentDate { get; set; }
    public string VehicleNumber { get; set; }
    public string ProductCategory { get; set; }
    public int ProductWeight { get; set; }
    
    public int SupplierId { get; set; }
    public int ProductId { get; set; }
    public int DepotItemId { get; set; }
    public int? CreatedById { get; set; }
    
    public virtual Supplier Supplier { get; set; }
    public virtual Product Product { get; set; }
    public virtual DepotItem DepotItem { get; set; }
    public virtual Employee? CreatedBy { get; set; }
}

