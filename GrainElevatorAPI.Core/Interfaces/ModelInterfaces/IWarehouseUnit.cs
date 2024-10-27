using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IWarehouseUnit
{
    int Id { get; set; }
    int SupplierId { get; set; }
    int ProductId { get; set; }
    
    ICollection<WarehouseProductCategory> ProductCategories { get; set; }
    ICollection<OutputInvoice> OutputInvoices { get; set; }
}