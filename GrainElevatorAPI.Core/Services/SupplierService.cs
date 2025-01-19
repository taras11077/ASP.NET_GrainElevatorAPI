using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class SupplierService : ISupplierService
{
    private readonly IRepository _repository;

    public SupplierService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<Supplier> CreateSupplierAsync(Supplier supplier, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            supplier.CreatedAt = DateTime.UtcNow;
            supplier.CreatedById = createdById;
            
            return await _repository.AddAsync(supplier, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при створенні Постачальника", ex);
        }
    }

    public async Task<Supplier> GetSupplierByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<Supplier>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Постачальника з ID {id}", ex);
        }
    }
    
    public async Task<IEnumerable<Supplier>> GetSuppliers(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Supplier>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Постачальників", ex);
        }
    }
    
    
    public async Task<(IEnumerable<Supplier>, int)> SearchSuppliersAsync(
        string? title,
        string? createdByName,
        int page,
        int size,
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken)
    {
        try
        {
            
            var query = _repository.GetAll<Supplier>()
                .Include(s => s.CreatedBy)
                .Where(to => to.RemovedAt == null);
            
            // Виклик методу фільтрації
            query = ApplyFilters(query, title, createdByName);

            // Виклик методу сортування
            query = ApplySorting(query, sortField, sortOrder);

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredSuppliers = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredSuppliers, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Постачальника", ex);
        }
    }
    
    
    private IQueryable<Supplier> ApplyFilters(
        IQueryable<Supplier> query,
        string? title,
        string? createdByName)
    {
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(s => s.Title == title);
        }
        if (!string.IsNullOrEmpty(createdByName))
        {
            query = query.Where(s => s.CreatedBy.LastName == createdByName);
        }
        
        return query;
    }
    
    private IQueryable<Supplier> ApplySorting(
        IQueryable<Supplier> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "title" => sortOrder == "asc"
                ? query.OrderBy(s => s.Title)
                : query.OrderByDescending(s => s.Title),
            "createdByName" => sortOrder == "asc"
                ? query.OrderBy(s => s.CreatedBy.LastName)
                : query.OrderByDescending(s => s.CreatedBy.LastName),
            _ => query // Якщо поле не визначене
        };
    }
    

    public async Task<Supplier> UpdateSupplierAsync(Supplier supplier, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            supplier.ModifiedAt = DateTime.UtcNow;
            supplier.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(supplier, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Постачальника з ID  {supplier.Id}", ex);
        }
    }

    public async Task<Supplier> SoftDeleteSupplierAsync(Supplier supplier, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            supplier.RemovedAt = DateTime.UtcNow;
            supplier.RemovedById = removedById;
            
            return await _repository.UpdateAsync(supplier, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Постачальника з ID  {supplier.Id}", ex);
        }
    }
    
    public async Task<Supplier> RestoreRemovedSupplierAsync(Supplier supplier, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            supplier.RemovedAt = null;
            supplier.RestoredAt = DateTime.UtcNow;
            supplier.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(supplier, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Постачальника з ID  {supplier.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteSupplierAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _repository.GetByIdAsync<Supplier>(id, cancellationToken);
            if (supplier != null)
            {
                await _repository.DeleteAsync<Supplier>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Постачальника з ID {id}", ex);
        }
    }
    
}