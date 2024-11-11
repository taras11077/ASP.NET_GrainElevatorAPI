using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IRegisterCalculator
{
    IInvoiceRegister CalcProductionBatch(ILaboratoryCard laboratoryCard, IInvoiceRegister invoiceRegister,
        IProductionBatch productionBatch);
}

