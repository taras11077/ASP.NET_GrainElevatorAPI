using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.Extensions.Logging;

namespace GrainElevatorAPI.Core.Services;

public class InputInvoiceService : IInputInvoiceService
{
    private readonly IRepository _repository;
    private readonly ILogger<InputInvoiceService> _logger;

    public InputInvoiceService(IRepository repository, ILogger<InputInvoiceService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    
    public async Task<InputInvoice> CreateInputInvoiceAsync(InputInvoice inputInvoice, int createdById)
    {
        try
        {
            inputInvoice.CreatedAt = DateTime.UtcNow;
            inputInvoice.CreatedById = createdById;
            
            await _repository.AddAsync(inputInvoice);
            await _repository.SaveChangesAsync();
            
            return inputInvoice;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервіса при додаванні Вхідної накладної", ex);
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
    public async Task<InputInvoice> GetInputInvoiceByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync<InputInvoice>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Вхідної накладної з ID {id}", ex);
        }
    }
    
    
    public IEnumerable<InputInvoice> SearchInputInvoices(
        int? id,
        string? invoiceNumber,
        DateTime? arrivalDate,
        string? vehicleNumber,
        int? physicalWeight,
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
            throw new Exception("Помилка сервіса при пошуку Вхідних накладних", ex);
        }
    }
    public async Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice, int modifiedById)
    {
        try
        {
            inputInvoice.ModifiedAt = DateTime.UtcNow;
            inputInvoice.ModifiedById = modifiedById;
            
            await _repository.UpdateAsync(inputInvoice);
            await _repository.SaveChangesAsync();
            
            return inputInvoice;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при оновленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }
    
    public async Task<InputInvoice> SoftDeleteInputInvoiceAsync(InputInvoice inputInvoice, int removedById)
    {
        try
        {
            inputInvoice.RemovedAt = DateTime.UtcNow;
            inputInvoice.RemovedById = removedById;
            
            await _repository.UpdateAsync(inputInvoice);
            await _repository.SaveChangesAsync();
            
            return inputInvoice;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при видаленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }
    
    public async Task<InputInvoice> RestoreRemovedInputInvoiceAsync(InputInvoice inputInvoice, int restoredById)
    {
        if (inputInvoice == null)
        {
            throw new ArgumentNullException(nameof(inputInvoice), "Вхідна накладна не може бути null.");
        }
        
        try
        {
            inputInvoice.RemovedAt = null;
            inputInvoice.RestoredAt = DateTime.UtcNow;
            inputInvoice.RestoreById = restoredById;
            
            await _repository.UpdateAsync(inputInvoice);
            await _repository.SaveChangesAsync();
            
            return inputInvoice;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при відновленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteInputInvoiceAsync(int id)
    {
        try
        {
            var supplier = await _repository.GetByIdAsync<InputInvoice>(id);
            if (supplier != null)
            {
                await _repository.DeleteAsync<InputInvoice>(id);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при видаленні Вхідної накладної з ID {id}", ex);
        }
    }
    
}