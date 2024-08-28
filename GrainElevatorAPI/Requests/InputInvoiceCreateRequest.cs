namespace GrainElevatorAPI.Requests;

public class InputInvoiceCreateRequest
{
    public int SupplierId { get; set; }
    public int ProductId { get; set; }
    public string InvoiceNumber { get; set; }
    public string VehicleNumber { get; set; } 
    public int PhysicalWeight { get; set; }
    
    public int CreatedById { get; set; } 
}