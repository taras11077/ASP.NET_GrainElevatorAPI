namespace GrainElevatorAPI.DTO.DTOs;

public class LaboratoryCardDto
{
    public int Id { get; set; }
	public string LabCardNumber { get; set; }
	public double WeedImpurity { get; set; }
	public double Moisture { get; set; }
	public double? GrainImpurity { get; set; }
	public string? SpecialNotes { get; set; }
	
    public bool? IsProduction { get; set; }
    
    public int InputInvoiceId { get; set; }
    public int? ProductionBatchId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
    public DateTime? RestoredAt { get; set; }
    
    public int CreatedById { get; set; }
    public int? ModifiedById { get; set; }
    public int? RemovedById { get; set; }
    public int? RestoreById { get; set; }
}