using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface ICompletionReportCalculator
{
    void CalculateWeights(IEnumerable<InvoiceRegister> registers, CompletionReport report);

    void CalculateTotalCost(CompletionReport report, PriceList priceList);

    int? MapOperationToReportField(TechnologicalOperation operation, CompletionReport report);
}

