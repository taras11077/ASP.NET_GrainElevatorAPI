﻿using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IInvoiceRegisterService
{
    Task<InvoiceRegister> CreateRegisterAsync(
        int supplierId,
        int productId,
        DateTime arrivalDate,
        double weedImpurityBase,
        double moistureBase,
        List<int> laboratoryCardIds,
        int createdById);
    Task<InvoiceRegister> GetRegisterByIdAsync(int id);
    Task<InvoiceRegister> UpdateRegisterAsync(InvoiceRegister invoiceRegister);
    Task<bool> DeleteRegisterAsync(int id);
    IQueryable<InvoiceRegister> GetRegisters(int page, int size);
    IEnumerable<InvoiceRegister> SearchRegisters(string registerNumber);
}