
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators;

public interface IProductionBatchCalculator
{
    void CalcProductionBatch(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IRegister register,
        ProductionBatch productionBatch);
}

