using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class InputInvoiceService : IInputInvoiceService
{
    private readonly IRepository _repository;

    public InputInvoiceService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<InputInvoice> AddInputInvoiceAsync(InputInvoice inputInvoice, int createdById)
    {
        try
        {
            inputInvoice.CreatedAt = DateTime.UtcNow;
            inputInvoice.CreatedById = createdById;
            
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
    
    public async Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice, int modifiedById)
    {
        try
        {
            inputInvoice.ModifiedAt = DateTime.UtcNow;
            inputInvoice.ModifiedById = modifiedById;
            
            return await _repository.Update(inputInvoice);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }
    
    public async Task<InputInvoice> SoftDeleteInputInvoiceAsync(InputInvoice inputInvoice, int removedById)
    {
        try
        {
            inputInvoice.RemovedAt = DateTime.UtcNow;
            inputInvoice.RemovedById = removedById;
            
            return await _repository.Update(inputInvoice);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }
    
    public async Task<InputInvoice> RestoreRemovedInputInvoiceAsync(InputInvoice inputInvoice, int restoredById)
    {
        try
        {
            inputInvoice.RemovedAt = null;
            inputInvoice.RestoredAt = DateTime.UtcNow;
            inputInvoice.RestoreById = restoredById;
            
            return await _repository.Update(inputInvoice);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при відновленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
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

    public IEnumerable<InputInvoice> SearchInputInvoices(
        int? id,
        string? invoiceNumber,
        DateTime? arrivalDate,
        string? vehicleNumber,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size)
    {
        try
        {
            // отримуємо всі накладні та конвертуємо у IQueryable для фільтрації
            var query = GetInputInvoices(page, size).AsQueryable();
            
            if (id.HasValue)
                query = query.Where(ii => ii.Id == id.Value);

            if (!string.IsNullOrEmpty(invoiceNumber))
                query = query.Where(ii => ii.InvoiceNumber == invoiceNumber);

            if (arrivalDate.HasValue)
                query = query.Where(ii => ii.ArrivalDate.Date == arrivalDate.Value.Date);

            if (!string.IsNullOrEmpty(vehicleNumber))
                query = query.Where(ii => ii.VehicleNumber == vehicleNumber);

            if (supplierId.HasValue)
                query = query.Where(ii => ii.SupplierId == supplierId.Value);

            if (productId.HasValue)
                query = query.Where(ii => ii.ProductId == productId.Value);

            if (createdById.HasValue)
                query = query.Where(ii => ii.CreatedById == createdById.Value);

            if (removedAt.HasValue)
                query = query.Where(ii => ii.RemovedAt == removedAt.Value);
            
            return query.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при пошуку вхідних накладних", ex);
        }
    }
}