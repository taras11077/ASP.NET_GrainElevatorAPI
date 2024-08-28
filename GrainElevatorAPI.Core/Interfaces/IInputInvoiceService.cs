using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IInputInvoiceService
{
    Task<InputInvoice> AddInputInvoiceAsync(InputInvoice inputInvoice);
    Task<InputInvoice> GetInputInvoiceAsync(int id);
    Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice);
    Task<bool> DeleteInputInvoiceAsync(int id);
    IEnumerable<InputInvoice> GetInputInvoices(int page, int size);
    IEnumerable<InputInvoice> SearchInputInvoice(string title);
}