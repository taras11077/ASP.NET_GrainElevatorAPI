using GrainElevatorAPI.Core.EnumsAndConstants;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators;

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
            report.PhysicalWeightReport = registers.Sum(r => (r.PhysicalWeightReg ?? 0) * 0.001);
            report.QuantitiesDryingReport = registers.Sum(r => r.QuantitiesDryingReg ?? 0);
            report.ShrinkageReport = registers.Sum(r => (r.ShrinkageReg ?? 0)*0.001);
            report.WasteReport = registers.Sum(r => (r.WasteReg ?? 0)*0.001);
            report.AccWeightReport = registers.Sum(r => (r.AccWeightReg ?? 0)*0.001);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Помилка під час обчислення вагових характеристик Акта виконаних робіт", ex);
        }
    }
    
    // створення відповідності технологічних операцій до полів звіту
    public double? MapOperationToReportField(TechnologicalOperation operation, CompletionReport report)
    {
        return operation.Title switch
        {
            TechnologicalOperationNames.Reception => report.PhysicalWeightReport,
            TechnologicalOperationNames.PrimaryCleaning => report.PhysicalWeightReport,
            TechnologicalOperationNames.Drying => report.QuantitiesDryingReport,
            TechnologicalOperationNames.Shipping => report.AccWeightReport,
            TechnologicalOperationNames.WasteDisposal => report.WasteReport,
            _ => null // якщо операція не знайдена, повертаємо null
        };
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
                    reportOperation.OperationCost = (decimal)reportOperation.Amount * matchingPriceListItem.OperationPrice;
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

