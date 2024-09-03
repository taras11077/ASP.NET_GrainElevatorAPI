using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IInputInvoiceService
{
    Task<InputInvoice> AddInputInvoiceAsync(InputInvoice inputInvoice);
    Task<InputInvoice> GetInputInvoiceByIdAsync(int id);
    Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice);
    Task<bool> DeleteInputInvoiceAsync(int id);
    IEnumerable<InputInvoice> GetInputInvoices(int page, int size);
    IEnumerable<InputInvoice> SearchInputInvoices(string invoiceNumber);
}