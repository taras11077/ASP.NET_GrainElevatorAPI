﻿using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators;

public class StandardRegisterCalculator : IRegisterCalculator
{
    public IInvoiceRegister CalcProductionBatch(ILaboratoryCard laboratoryCard,
        IInvoiceRegister invoiceRegister, IProductionBatch productionBatch)
    {
        var inputInvoice = (laboratoryCard as LaboratoryCard)?.InputInvoice;
        
        if (inputInvoice != null && inputInvoice.PhysicalWeight <= 0)
            throw new ArgumentException("Physical weight must be greater than zero.",
                nameof(inputInvoice.PhysicalWeight));

        // розрахунок втрати ваги при очищенні (Waste)
        productionBatch.Waste = laboratoryCard.WeedImpurity <= invoiceRegister.WeedImpurityBase
            ? 0
            : (int)(inputInvoice.PhysicalWeight *
                    (1 - (100 - laboratoryCard.WeedImpurity) / (100 - invoiceRegister.WeedImpurityBase)));

        // розрахунок втрати ваги при сушінні (Shrinkage)
        productionBatch.Shrinkage = laboratoryCard.Moisture <= invoiceRegister.MoistureBase
            ? 0
            : (int)((inputInvoice.PhysicalWeight - productionBatch.Waste) *
                    (1 - (100 - laboratoryCard.Moisture) / (100 - invoiceRegister.MoistureBase)));

        // розрахунок залікової ваги (AccountWeight)
        productionBatch.AccountWeight = inputInvoice.PhysicalWeight - productionBatch.Waste - productionBatch.Shrinkage;

        // розрахунок кількості сушіння QuantitiesDrying
        var quantitiesDrying = (inputInvoice.PhysicalWeight - productionBatch.Waste) *
            (laboratoryCard.Moisture - invoiceRegister.MoistureBase) / 1000;
        productionBatch.QuantitiesDrying = productionBatch.Shrinkage != 0 ? quantitiesDrying : 0;

        // додавання Партии в Реєстр
        return AddProductionBatch(inputInvoice, productionBatch, invoiceRegister);
    }

    public IInvoiceRegister AddProductionBatch(IInputInvoice inputInvoice, IProductionBatch productionBatch,
        IInvoiceRegister invoiceRegister)
    {
        if (productionBatch is ProductionBatch batch)
            invoiceRegister.ProductionBatches.Add(batch);

        if (invoiceRegister is InvoiceRegister register)
        {
            register.PhysicalWeightReg += inputInvoice.PhysicalWeight;
            register.ShrinkageReg += productionBatch.Shrinkage;
            register.WasteReg += productionBatch.Waste;
            register.AccWeightReg += productionBatch.AccountWeight;
            register.QuantitiesDryingReg += productionBatch.QuantitiesDrying;
        }
        return invoiceRegister;
    }
}


