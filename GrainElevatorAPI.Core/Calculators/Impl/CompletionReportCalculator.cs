﻿using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators.Impl;

public class CompletionReportCalculator : ICompletionReportCalculator
{
    // розрахунок вагових характеристик
    public void CalculateWeights(IEnumerable<InvoiceRegister> registers, CompletionReport report)
    {
        if (registers == null)
            throw new ArgumentNullException(nameof(registers), "Список реєстрів не може бути null");
        if (report == null)
            throw new ArgumentNullException(nameof(report), "Акт виконаних робіт не може бути null");

        try
        {
            report.PhysicalWeightReport = registers.Sum(r => r.PhysicalWeightReg ?? 0);
            report.QuantitiesDryingReport = (int)registers.Sum(r => r.QuantitiesDryingReg ?? 0);
            report.ShrinkageReport = registers.Sum(r => r.ShrinkageReg ?? 0);
            report.WasteReport = registers.Sum(r => r.WasteReg ?? 0);
            report.AccWeightReport = registers.Sum(r => r.AccWeightReg ?? 0);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Помилка під час обчислення вагових характеристик Акта виконаних робіт", ex);
        }
    }

    // розрахунок загальної вартості
    public void CalculateTotalCost(CompletionReport completionReport, PriceList priceList)
    {
        if (priceList == null)
            throw new ArgumentNullException(nameof(priceList), "Прайс-лист не може бути null");
        if (completionReport == null)
            throw new ArgumentNullException(nameof(completionReport), "Акт виконаних робіт не може бути null");

        try
        {
            decimal totalCost = 0;

            foreach (var reportOperation in completionReport.CompletionReportOperations)
            {
                var matchingPriceListItem = priceList.PriceListItems
                    .FirstOrDefault(item => item.TechnologicalOperationId == reportOperation.TechnologicalOperationId);

                if (matchingPriceListItem != null)
                {
                    // Обчислюємо вартість для конкретної операції
                    reportOperation.OperationCost = reportOperation.Amount * matchingPriceListItem.OperationPrice;
                    totalCost += reportOperation.OperationCost.Value;
                }
            }

            completionReport.TotalCost = totalCost;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Помилка під час обчислення вартості виконаних робіт", ex);
        }
    }



}

