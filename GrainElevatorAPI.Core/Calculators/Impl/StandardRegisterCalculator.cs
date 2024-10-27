using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators.Impl;

public class StandardRegisterCalculator : IRegisterCalculator 
{
    public void CalcProductionBatch(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IInvoiceRegister invoiceRegister, IProductionBatch productionBatch)
    {
        if (inputInvoice.PhysicalWeight <= 0)
            throw new ArgumentException("Physical weight must be greater than zero.", nameof(inputInvoice.PhysicalWeight));

        // розрахунок втрати ваги при очищенні (Waste)
        productionBatch.Waste = laboratoryCard.WeedImpurity <= invoiceRegister.WeedImpurityBase 
            ? 0 
            : (int)(inputInvoice.PhysicalWeight * (1 - (100 - laboratoryCard.WeedImpurity) / (100 - invoiceRegister.WeedImpurityBase)));

        // розрахунок втрати ваги при сушінні (Shrinkage)
        productionBatch.Shrinkage = laboratoryCard.Moisture <= invoiceRegister.MoistureBase
            ? 0
            : (int)((inputInvoice.PhysicalWeight - productionBatch.Waste) * (1 - (100 - laboratoryCard.Moisture) / (100 - invoiceRegister.MoistureBase)));

        // розрахунок залікової ваги (AccountWeight)
        productionBatch.AccountWeight = inputInvoice.PhysicalWeight - productionBatch.Waste - productionBatch.Shrinkage;

        // розрахунок кількості сушіння QuantitiesDrying
        var quantitiesDrying = (inputInvoice.PhysicalWeight - productionBatch.Waste) *
            (laboratoryCard.Moisture - invoiceRegister.MoistureBase) / 1000;
        productionBatch.QuantitiesDrying = productionBatch.Shrinkage != 0 ? quantitiesDrying : 0;
        
        // додавання Партии в Реєстр
        AddProductionBatch(inputInvoice, laboratoryCard, productionBatch, invoiceRegister);
    }

    public void AddProductionBatch(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IProductionBatch productionBatch, IInvoiceRegister invoiceRegister)
    {
        if(productionBatch is ProductionBatch batch)
            invoiceRegister.ProductionBatches.Add(batch);

        if (invoiceRegister is InvoiceRegister register)
        {
            register.PhysicalWeightReg += inputInvoice.PhysicalWeight;
            register.ShrinkageReg += productionBatch.Shrinkage;
            register.WasteReg += productionBatch.Waste;
            register.AccWeightReg += productionBatch.AccountWeight;
            register.QuantitiesDryingReg += productionBatch.QuantitiesDrying;  
        }
       
    }
    
}


