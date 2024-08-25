using GrainElevatorAPI.Core.Calculators.Impl;

namespace GrainElevatorAPI.Core.Models;

public class ProductionBatch
{
    public int Id { get; set; }
    public double WeedinessBase { get; set; }
    public double MoistureBase { get; set; }
    public int Waste { get; set; }
    public int Shrinkage { get; set; }
    public int AccountWeight { get; set; }

    public int LaboratoryCardId { get; set; }
    public int RegisterId { get; set; }
    public virtual LaboratoryCard LaboratoryCard { get; set; }
    public virtual Register Register { get; set; }

}

