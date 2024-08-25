namespace GrainElevatorAPI.Core.Models;

public partial class ProductCategory
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    
    public int DepotItemId { get; set; }

    public virtual DepotItem DepotItem { get; set; }
}

