
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators;

public interface IRegisterCalculator
{
    void CalcProductionBatch(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IInvoiceRegister invoiceRegister,
        IProductionBatch productionBatch);
}

