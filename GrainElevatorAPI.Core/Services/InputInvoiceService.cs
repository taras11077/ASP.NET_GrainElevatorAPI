using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class InputInvoiceService : IInputInvoiceService
{
    private readonly IRepository _repository;

    public InputInvoiceService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public Task<InputInvoice> AddInputInvoice(string title)
    {
        throw new NotImplementedException();
    }

    public Task<InputInvoice> GetInputInvoiceById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<InputInvoice> UpdateInputInvoice(InputInvoice inputInvoice)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteInputInvoice(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<InputInvoice> GetInputInvoice(int page, int size)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<InputInvoice> SearchInputInvoice(string title)
    {
        throw new NotImplementedException();
    }
}