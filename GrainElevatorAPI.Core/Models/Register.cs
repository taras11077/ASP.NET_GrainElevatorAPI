using System.Text;

namespace GrainElevatorAPI.Core.Models;

public class Register
{
    public int Id { get; set; }
    public int RegisterNumber { get; set; }
    public DateTime ArrivalDate { get; set; }
    public int PhysicalWeightReg { get; set; }
    public int ShrinkageReg { get; set; }
    public int WasteReg { get; set; }
    public int AccWeightReg { get; set; }
    public double QuantityesDryingReg { get; set; }
    
    public int SupplierId { get; set; }
    public int ProductId { get; set; }
    public int? CompletionReportId { get; set; }
    public int? CreatedById { get; set; }
    
    public virtual Supplier Supplier { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public virtual CompletionReport? CompletionReport { get; set; }
    public virtual Employee? CreatedBy { get; set; }

    public virtual ICollection<ProductionBatch> ProductionBatches { get; set; } = new List<ProductionBatch>();
}

