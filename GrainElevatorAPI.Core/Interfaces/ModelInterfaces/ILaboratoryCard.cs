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
    
}