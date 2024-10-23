
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators;

public interface IProductionButchCalculator
{
    void CalcResultProduction(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IRegister register,
        ProductionBatch productionBatch);
}

