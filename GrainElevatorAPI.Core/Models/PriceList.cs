namespace GrainElevatorAPI.Core.Models;

public class PriceList
{
    public int Id { get; set; }
    public string Product { get; set; } = null!;
    public int? CreatedByInt { get; set; }

    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<PriceByOperation> PriceByOperations { get; set; } = new List<PriceByOperation>();
    public virtual Employee? CreatedBy { get; set; }

}

