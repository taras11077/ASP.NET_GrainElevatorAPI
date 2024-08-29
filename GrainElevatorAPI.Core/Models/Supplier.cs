namespace GrainElevatorAPI.Core.Models;

public class Supplier
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public bool? Removed { get; set; } = false;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
    public DateTime? RestoredAt { get; set; }
    
    public int CreatedById { get; set; }
    public int? ModifiedById { get; set; }
    public int? RemovedById { get; set; }
    public int? RestoreById { get; set; }
    
    public virtual Employee CreatedBy { get; set; }
    public virtual Employee? ModifiedBy { get; set; }
    public virtual Employee? RemovedBy { get; set; }
    public virtual Employee? RestoreBy { get; set; }

    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<DepotItem> DepotItems { get; set; } = new List<DepotItem>();
    public virtual ICollection<InputInvoice> InputInvoices { get; set; } = new List<InputInvoice>();
    public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();
    public virtual ICollection<Register> Registers { get; set; } = new List<Register>();
    
}

