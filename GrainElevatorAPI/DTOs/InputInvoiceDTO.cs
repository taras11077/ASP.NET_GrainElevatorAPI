namespace GrainElevatorAPI.DTOs;

public class InputInvoiceDTO
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
}