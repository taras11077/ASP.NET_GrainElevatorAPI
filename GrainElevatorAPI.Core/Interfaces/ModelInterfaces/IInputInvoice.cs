using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IInputInvoice
{
	int Id { get; set; }
	string InvoiceNumber { get; set; }
	DateTime ArrivalDate { get; set; }
	string? VehicleNumber { get; set; } 
	int PhysicalWeight { get; set; }

    int? LaboratoryCardId { get; set; }
    int SupplierId { get; set; }
    int ProductId { get; set; }
    
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
    DateTime? RemovedAt { get; set; }
    DateTime? RestoredAt { get; set; }
    
    int CreatedById { get; set; }
    int? ModifiedById { get; set; }
    int? RemovedById { get; set; }
    int? RestoreById { get; set; }
}