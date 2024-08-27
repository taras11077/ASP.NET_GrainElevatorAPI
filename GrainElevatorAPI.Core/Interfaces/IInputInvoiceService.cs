using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IInputInvoiceService
{
    Task<InputInvoice> AddInputInvoice(string  title);
    Task<InputInvoice> GetInputInvoiceById(int id);
    Task<InputInvoice> UpdateInputInvoice(InputInvoice inputInvoice);
    Task<bool> DeleteInputInvoice(int id);
    IEnumerable<InputInvoice> GetInputInvoice(int page, int size);
    IEnumerable<InputInvoice> SearchInputInvoice(string title);
}