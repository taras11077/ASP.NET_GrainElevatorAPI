using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IInvoiceRegisterService
{
    Task<InvoiceRegister> CreateRegisterAsync(
        int supplierId,
        int productId,
        DateTime arrivalDate,
        double weedImpurityBase,
        double moistureBase,
        IEnumerable<int> laboratoryCardIds,
        int createdById,
        CancellationToken cancellationToken);
    Task<InvoiceRegister> GetRegisterByIdAsync(int id, CancellationToken cancellationToken);
    Task<InvoiceRegister> UpdateRegisterAsync(InvoiceRegister invoiceRegister, CancellationToken cancellationToken);
    Task<bool> DeleteRegisterAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<InvoiceRegister>> GetRegisters(int page, int size, CancellationToken cancellationToken);
    Task<IEnumerable<InvoiceRegister>> SearchRegisters(string registerNumber, CancellationToken cancellationToken);
}