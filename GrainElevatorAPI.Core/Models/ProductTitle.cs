namespace GrainElevatorAPI.Core.Models;

public class ProductTitle
{
    public int Id { get; set; }
    public string Title { get; set; }

    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<DepotItem> DepotItems { get; set; } = new List<DepotItem>();
    public virtual ICollection<InputInvoice> InputInvoices { get; set; } = new List<InputInvoice>();
    public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();
    public virtual ICollection<Register> Registers { get; set; } = new List<Register>();
}

