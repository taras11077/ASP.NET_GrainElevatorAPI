namespace GrainElevatorAPI.Core.Models;

public partial class DepotItem
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public int ProductTitleId { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();

    public virtual ProductTitle ProductTitle { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
