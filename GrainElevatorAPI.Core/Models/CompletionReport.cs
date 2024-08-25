using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Calculators.Impl;

namespace GrainElevatorAPI.Core.Models;

public class CompletionReport
{
    public int Id { get; set; }
    public int ReportNumber { get; set; }
    public DateTime ReportDate { get; set; }
    public double QuantityesDrying { get; set; }
    public double PhysicalWeightReport { get; set; }
    public bool IsFinalized { get; set; }
    
    public int SupplierId { get; set; }
    public int ProductTitleId { get; set; }
    public int? PriceListId { get; set; }
    public int CreatedById { get; set; }
    
    public virtual Supplier Supplier { get; set; }
    public virtual ProductTitle ProductTitle { get; set; }
    public virtual PriceList? PriceList { get; set; }
    public virtual Employee? CreatedBy { get; set; }
    
    public virtual ICollection<Register> Registers { get; set; } = new List<Register>();
    public virtual ICollection<TechnologicalOperation> TechnologicalOperations { get; set; } = new List<TechnologicalOperation>();
}

