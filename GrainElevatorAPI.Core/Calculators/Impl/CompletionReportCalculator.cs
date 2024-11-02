using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators.Impl;

public class CompletionReportCalculator : ICompletionReportCalculator
{
    public CompletionReport CompletionReport { get; set; }

    public CompletionReportCalculator(CompletionReport cr)
    {
        CompletionReport = cr;
    }

    // рассчет общего Физического веса всех Реестров
    public double CalcSumWeightReport()
    {
        if (CompletionReport.Registers.Count == 0)
            return 0;

        foreach (InvoiceRegister reg in CompletionReport.Registers)
            CompletionReport.ReportPhysicalWeight += (double)reg.PhysicalWeightReg / 1000;

        return CompletionReport.ReportPhysicalWeight.Value;
    }

    // расчет тонно/процентов сушки по каждой ППП всех Реестров Акта
    public double CalcDryingQuantity()
    {
        if (CompletionReport.Registers.Count == 0)
            return 0;

        try
        {
            foreach (InvoiceRegister reg in CompletionReport.Registers)
            {
                //    if ((reg.ProductionBatches as List<ProductionBatch>) is null)
                //        return 0.0;

                //    (reg.ProductionBatches as List<ProductionBatch>)?.ForEach(p =>
                //    {
                //        if (p.Shrinkage != 0)
                CompletionReport.ReportQuantitiesDrying += reg.QuantitiesDryingReg;
            }

            return Math.Round(CompletionReport.ReportQuantitiesDrying.Value, 2);

        }
        catch (Exception)
        {
            // TODO
            throw;
        }
    }

    //рассчет финансовой части Акта доработка по заданному Прайсу
    public void CalcByPrice(PriceList pl)
    {
        try
        {
            (CompletionReport.CompletionReportItems as List<CompletionReportOperation>)?.ForEach(op =>
            {
                foreach (var p in pl.PriceListItems)
                    if (op.OperationName == p.OperationName)
                    {
                        // op.Price = p.OperationPrice;
                        // op.TotalCost = Math.Round(op.Amount * op.Price, 2);
                    }
            });
        }
        catch (Exception)
        {
            // TODO
            throw;
        }
    }



}

