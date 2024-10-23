using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators.Impl;

public class StandardProductionButchCalculator : IProductionButchCalculator
{
    public ProductionBatch ProductionBatch { get; private set; }

    public void CalcResultProduction(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IRegister register, ProductionBatch productionBatch)
    {
        ProductionBatch = productionBatch;

        if (laboratoryCard.WeedImpurity <= register.WeedImpurityBase)
            ProductionBatch.Waste = 0;
        else
            ProductionBatch.Waste = (int)(inputInvoice.PhysicalWeight * (1 - (100 - laboratoryCard.WeedImpurity) / (100 - register.WeedImpurityBase)));

        if (laboratoryCard.Moisture <= register.MoistureBase)
            ProductionBatch.Shrinkage = 0;
        else
            ProductionBatch.Shrinkage = (int)((inputInvoice.PhysicalWeight - ProductionBatch.Waste) * (1 - (100 - laboratoryCard.Moisture) / (100 - register.MoistureBase)));

        ProductionBatch.AccountWeight = inputInvoice.PhysicalWeight - ProductionBatch.Waste - ProductionBatch.Shrinkage;
    }
}

