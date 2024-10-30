﻿using GrainElevatorAPI.Core.Interfaces;
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
            throw new Exception("Помилка при додаванні Постачальника", ex);
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
    
    public async Task<Supplier> GetSupplierByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<Supplier>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Постачальника з ID {id}", ex);
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
            throw new Exception($"Помилка при отриманні Постачальника з назвою {title}", ex);
        }
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
            throw new Exception($"Помилка при оновленні Постачальника з ID  {supplier.Id}", ex);
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
            throw new Exception($"Помилка при видаленні Постачальника з ID  {supplier.Id}", ex);
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
            throw new Exception($"Помилка при відновленні Постачальника з ID  {supplier.Id}", ex);
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
            throw new Exception($"Помилка при видаленні Постачальника з ID {id}", ex);
        }
    }
    
}