using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class SupplierService : ISupplierService
{
    private readonly IRepository _repository;

    public SupplierService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<Supplier> CreateSupplierAsync(Supplier supplier, int createdById)
    {
        try
        {
            supplier.CreatedAt = DateTime.UtcNow;
            supplier.CreatedById = createdById;
            
            await _repository.AddAsync(supplier);
            await _repository.SaveChangesAsync();
            
            return supplier;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при додаванні Постачальника", ex);
        }
    }

    public IEnumerable<Supplier> GetSuppliers(int page, int size)
    {
        try
        {
            return _repository.GetAll<Supplier>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Постачальників", ex);
        }
    }
    
    public async Task<Supplier> GetSupplierByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync<Supplier>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Постачальника з ID {id}", ex);
        }
    }
    
    public IEnumerable<Supplier> SearchSupplier(string title)
    {
        try
        {
            return _repository.GetAll<Supplier>()
                .Where(s => s.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Постачальника з назвою {title}", ex);
        }
    }

    public async Task<Supplier> UpdateSupplierAsync(Supplier supplier, int modifiedById)
    {
        try
        {
            supplier.ModifiedAt = DateTime.UtcNow;
            supplier.ModifiedById = modifiedById;
            
            await _repository.UpdateAsync(supplier);
            await _repository.SaveChangesAsync();
            
            return supplier;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Постачальника з ID  {supplier.Id}", ex);
        }
    }

    public async Task<Supplier> SoftDeleteSupplierAsync(Supplier supplier, int removedById)
    {
        try
        {
            supplier.RemovedAt = DateTime.UtcNow;
            supplier.RemovedById = removedById;
            
            await _repository.UpdateAsync(supplier);
            await _repository.SaveChangesAsync();
            
            return supplier;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Постачальника з ID  {supplier.Id}", ex);
        }
    }
    
    public async Task<Supplier> RestoreRemovedSupplierAsync(Supplier supplier, int restoredById)
    {
        try
        {
            supplier.RemovedAt = null;
            supplier.RestoredAt = DateTime.UtcNow;
            supplier.RestoreById = restoredById;
            
            await _repository.UpdateAsync(supplier);
            await _repository.SaveChangesAsync();
            
            return supplier;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Постачальника з ID  {supplier.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteSupplierAsync(int id)
    {
        try
        {
            var supplier = await _repository.GetByIdAsync<Supplier>(id);
            if (supplier != null)
            {
                await _repository.DeleteAsync<Supplier>(id);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при hard-видаленні Постачальника з ID {id}", ex);
        }
    }
    
}