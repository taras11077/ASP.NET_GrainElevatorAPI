
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Calculators;

public interface IRegisterCalculator
{
    IInvoiceRegister CalcProductionBatch(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IInvoiceRegister invoiceRegister,
        IProductionBatch productionBatch);
}

