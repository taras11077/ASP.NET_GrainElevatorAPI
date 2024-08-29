using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class SupplierService : ISupplierService
{
    private readonly IRepository _repository;

    public SupplierService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<Supplier> AddSupplierAsync(Supplier supplier)
    {
        try
        {
            return await _repository.Add(supplier);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні Постачальника", ex);
        }
    }

    public async Task<Supplier> GetSupplierByIdAsync(int id)
    {
        try
        {
            return await _repository.GetById<Supplier>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Постачальника з ID {id}", ex);
        }
    }

    public async Task<Supplier> UpdateSupplierAsync(Supplier supplier)
    {
        try
        {
            return await _repository.Update(supplier);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Постачальника з ID  {supplier.Id}", ex);
        }
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        try
        {
            var supplier = await _repository.GetById<Supplier>(id);
            if (supplier != null)
            {
                await _repository.Delete<Supplier>(id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Постачальника з ID {id}", ex);
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
            throw new Exception("Помилка при отриманні списку Постачальників", ex);
        }
    }

    public IEnumerable<Supplier> SearchSupplier(string title)
    {
        try
        {
            return _repository.GetAll<Supplier>()
                .Where(r => r.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Постачальника з назвою {title}", ex);
        }
    }
}