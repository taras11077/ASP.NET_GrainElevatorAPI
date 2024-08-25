namespace GrainElevatorAPI.Core.Models;

public class LaboratoryCard
{
    public int Id { get; set; }
    public int LabCardNumber { get; set; }
    public double Weediness { get; set; }
    public double Moisture { get; set; }
    public double? GrainImpurity { get; set; }
    public string? SpecialNotes { get; set; }
    public bool? IsProduction { get; set; }
    public int CreatedById { get; set; }

    public virtual Employee? CreatedBy { get; set; }
    public virtual InputInvoice InputInvoice { get; set; }
    public virtual ProductionBatch? ProductionBatch { get; set; }
  
}