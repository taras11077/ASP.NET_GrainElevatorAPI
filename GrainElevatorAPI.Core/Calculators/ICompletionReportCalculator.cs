using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators;

public interface ICompletionReportCalculator
{
    void CalculateWeights(IEnumerable<InvoiceRegister> registers, CompletionReport report);

    void CalculateTotalCost(CompletionReport report, PriceList priceList);
}

