namespace GrainElevatorAPI.Requests;

public class InputInvoiceUpdateRequest
{
    public string? InvoiceNumber { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public int? SupplierId { get; set; }
    public int? ProductId { get; set; }

    public string? VehicleNumber { get; set; } 
    public int? PhysicalWeight { get; set; }
}