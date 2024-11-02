using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators;

public interface ICompletionReportCalculator
{
    double CalcSumWeightReport();
    double CalcDryingQuantity();
    void CalcByPrice(PriceList pl);
}

