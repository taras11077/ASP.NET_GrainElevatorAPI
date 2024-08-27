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
    
    
    public async Task<Supplier> AddSupplierAsync(string title)
    {
        try
        {
            var newSupplier = new Supplier{ Title = title };
            
            return await _repository.Add(newSupplier);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні постачальника", ex);
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
            throw new Exception($"Помилка при отриманні постачальника з ID {id}", ex);
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
            throw new Exception($"Помилка при оновленні постачальника з ID  {supplier.Id}", ex);
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
            throw new Exception($"Помилка при видаленні постачальника з ID {id}", ex);
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
            throw new Exception("Помилка при отриманні списку постачальників", ex);
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
            throw new Exception($"Помилка при отриманні постачальника з назвою {title}", ex);
        }
    }
}