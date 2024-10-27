namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IOutputInvoice
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
}