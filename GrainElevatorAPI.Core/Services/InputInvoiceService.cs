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
    
    
    public async Task<InputInvoice> AddInputInvoiceAsync(InputInvoice inputInvoice)
    {
        try
        {
            return await _repository.Add(inputInvoice);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні Вхідної накладної", ex);
        }
    }

    public async Task<InputInvoice> GetInputInvoiceByIdAsync(int id)
    {
        try
        {
            return await _repository.GetById<InputInvoice>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Вхідної накладної з ID {id}", ex);
        }
    }

    public async Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice)
    {
        try
        {
            return await _repository.Update(inputInvoice);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }

    public async Task<bool> DeleteInputInvoiceAsync(int id)
    {
        try
        {
            var supplier = await _repository.GetById<InputInvoice>(id);
            if (supplier != null)
            {
                await _repository.Delete<InputInvoice>(id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Вхідної накладної з ID {id}", ex);
        }
    }

    public IEnumerable<InputInvoice> GetInputInvoices(int page, int size)
    {
        try
        {
            return _repository.GetAll<InputInvoice>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку Вхідних Накладних", ex);
        }
    }

    public IEnumerable<InputInvoice> SearchInputInvoice(string invoiceNumber)
    {
        try
        {
            return _repository.GetAll<InputInvoice>()
                .Where(inv => inv.InvoiceNumber.ToLower().Contains(invoiceNumber.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Вхідної накладної за номером {invoiceNumber}", ex);
        }
    }
}