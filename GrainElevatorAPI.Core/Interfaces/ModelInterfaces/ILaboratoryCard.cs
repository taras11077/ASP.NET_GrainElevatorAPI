namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ILaboratoryCard
{
	int Id { get; set; }
	string LabCardNumber { get; set; }
	double WeedImpurity { get; set; }
	double Moisture { get; set; }
	double? GrainImpurity { get; set; }
	string? SpecialNotes { get; set; }
    bool? IsProduction { get; set; }
    
    int InputInvoiceId { get; set; }
    int? ProductionBatchId { get; set; }
    
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
    DateTime? RemovedAt { get; set; }
    DateTime? RestoredAt { get; set; }
    
    int CreatedById { get; set; }
    int? ModifiedById { get; set; }
    int? RemovedById { get; set; }
    int? RestoreById { get; set; }
}