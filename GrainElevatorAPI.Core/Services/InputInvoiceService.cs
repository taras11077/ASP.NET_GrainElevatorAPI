﻿using GrainElevatorAPI.Core.Interfaces;
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
    
    
    public async Task<InputInvoice> AddInputInvoiceAsync(InputInvoice inputInvoice, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            inputInvoice.CreatedAt = DateTime.UtcNow;
            inputInvoice.CreatedById = createdById;
            
            return await _repository.AddAsync(inputInvoice, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні Вхідної накладної", ex);
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
    public async Task<InputInvoice> GetInputInvoiceByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<InputInvoice>(id, cancellationToken);
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
            
            if (physicalWeight.HasValue)
                query = query.Where(ii => ii.PhysicalWeight == physicalWeight.Value);

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
            throw new Exception("Помилка при пошуку Вхідних накладних", ex);
        }
    }
    public async Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            inputInvoice.ModifiedAt = DateTime.UtcNow;
            inputInvoice.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(inputInvoice, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }
    
    public async Task<InputInvoice> SoftDeleteInputInvoiceAsync(InputInvoice inputInvoice, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            inputInvoice.RemovedAt = DateTime.UtcNow;
            inputInvoice.RemovedById = removedById;
            
            return await _repository.UpdateAsync(inputInvoice, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }
    
    public async Task<InputInvoice> RestoreRemovedInputInvoiceAsync(InputInvoice inputInvoice, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            inputInvoice.RemovedAt = null;
            inputInvoice.RestoredAt = DateTime.UtcNow;
            inputInvoice.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(inputInvoice, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при відновленні Вхідної накладної з ID  {inputInvoice.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteInputInvoiceAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _repository.GetByIdAsync<InputInvoice>(id, cancellationToken);
            if (supplier != null)
            {
                await _repository.DeleteAsync<InputInvoice>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Вхідної накладної з ID {id}", ex);
        }
    }
    
}