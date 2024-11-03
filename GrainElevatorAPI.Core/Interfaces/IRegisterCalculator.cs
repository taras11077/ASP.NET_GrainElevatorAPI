using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IRegisterCalculator
{
    IInvoiceRegister CalcProductionBatch(IInputInvoice inputInvoice, ILaboratoryCard laboratoryCard, IInvoiceRegister invoiceRegister,
        IProductionBatch productionBatch);
}

