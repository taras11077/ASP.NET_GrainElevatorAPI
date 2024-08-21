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

        foreach (Register reg in CompletionReport.Registers)
            CompletionReport.PhysicalWeightReport += (double)reg.PhysicalWeightReg / 1000;

        return CompletionReport.PhysicalWeightReport;
    }

    // расчет тонно/процентов сушки по каждой ППП всех Реестров Акта
    public double CalcDryingQuantity()
    {
        if (CompletionReport.Registers.Count == 0)
            return 0;

        try
        {
            foreach (Register reg in CompletionReport.Registers)
            {
                //    if ((reg.ProductionBatches as List<ProductionBatch>) is null)
                //        return 0.0;

                //    (reg.ProductionBatches as List<ProductionBatch>)?.ForEach(p =>
                //    {
                //        if (p.Shrinkage != 0)
                CompletionReport.QuantityesDrying += reg.QuantityesDryingReg;
            }

            return Math.Round(CompletionReport.QuantityesDrying, 2);

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
            (CompletionReport.TechnologicalOperations as List<TechnologicalOperation>)?.ForEach(op =>
            {
                foreach (var p in pl.PriceByOperations)
                    if (op.Title == p.OperationTitle)
                    {
                        op.Price = p.OperationPrice;
                        op.TotalCost = Math.Round(op.Amount * op.Price, 2);
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

