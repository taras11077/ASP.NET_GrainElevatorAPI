using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators.Impl;

public class StandardProductionBatchCalculator : IProductionBatchCalculator 
{
    public void CalcProductionBatch(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IRegister register, ProductionBatch productionBatch)
    {
        if (inputInvoice.PhysicalWeight <= 0)
            throw new ArgumentException("Physical weight must be greater than zero.", nameof(inputInvoice.PhysicalWeight));

        // розрахунок втрати ваги при очищенні (Waste)
        productionBatch.Waste = laboratoryCard.WeedImpurity <= register.WeedImpurityBase 
            ? 0 
            : (int)(inputInvoice.PhysicalWeight * (1 - (100 - laboratoryCard.WeedImpurity) / (100 - register.WeedImpurityBase)));

        // розрахунок втрати ваги при сушінні (Shrinkage)
        productionBatch.Shrinkage = laboratoryCard.Moisture <= register.MoistureBase
            ? 0
            : (int)((inputInvoice.PhysicalWeight - productionBatch.Waste) * (1 - (100 - laboratoryCard.Moisture) / (100 - register.MoistureBase)));

        // розрахунок залікової ваги (AccountWeight)
        productionBatch.AccountWeight = inputInvoice.PhysicalWeight - productionBatch.Waste - productionBatch.Shrinkage;

        // розрахунок кількості сушіння QuantitiesDrying
        var quantitiesDrying = (inputInvoice.PhysicalWeight - productionBatch.Waste) *
            (laboratoryCard.Moisture - register.MoistureBase) / 1000;
        productionBatch.QuantitiesDrying = productionBatch.Shrinkage != 0 ? quantitiesDrying : 0;
    }
}


