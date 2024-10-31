using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GrainElevatorAPI.Core.Services;

public class OutputInvoiceService : IOutputInvoiceService
{
    private readonly IRepository _repository;
    private readonly ILogger<OutputInvoiceService> _logger;

    public OutputInvoiceService(IRepository repository, ILogger<OutputInvoiceService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<OutputInvoice> CreateOutputInvoiceAsync(
    string invoiceNumber,
    string vehicleNumber,
    int supplierId,
    int productId,
    string productCategory,
    int productWeight,
    int createdById, 
    CancellationToken cancellationToken)
{
    await _repository.BeginTransactionAsync(cancellationToken);

    try
    {

        var warehouseUnit = await _repository.GetAll<WarehouseUnit>()
            .FirstOrDefaultAsync(w => w.SupplierId == supplierId && w.ProductId == productId, cancellationToken);

        if (warehouseUnit == null)
        {
            throw new Exception($"Складський юніт із SupplierId {supplierId} та ProductId {productId} не знайдено.");
        }
        
        var productCategoryEntity = await _repository.GetAll<WarehouseProductCategory>()
            .FirstOrDefaultAsync(pc => pc.Title == productCategory && pc.WarehouseUnitId == warehouseUnit.Id, cancellationToken);

        if (productCategoryEntity == null)
        {
            throw new Exception($"Категорію продукту {productCategory} не знайдено на складі.");
        }

        // перевірка доступного залишку продукту
        if (productCategoryEntity.Value == null || productCategoryEntity.Value < productWeight)
        {
            throw new Exception($"Недостатньо продукції в категорії {productCategory}. Доступно: {productCategoryEntity.Value}");
        }

        // зменшення значення категорії продукту
        productCategoryEntity.Value -= productWeight;
        await _repository.UpdateAsync(productCategoryEntity, cancellationToken);

        // створення видаткової накладної
        var outputInvoice = new OutputInvoice
        {
            InvoiceNumber = invoiceNumber,
            VehicleNumber = vehicleNumber,
            SupplierId = supplierId,
            ProductId = productId,
            ProductCategory = productCategory,
            ProductWeight = productWeight,
            ShipmentDate = DateTime.UtcNow,
            WarehouseUnitId = warehouseUnit.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedById = createdById,
        };

        var addedInvoice = await _repository.AddAsync(outputInvoice, cancellationToken);

        // Коміт транзакції
        await _repository.CommitTransactionAsync(cancellationToken);

        return addedInvoice;
    }
    catch (Exception ex)
    {
        await _repository.RollbackTransactionAsync(cancellationToken);
        throw new Exception("Помилка сервісу при додаванні видаткової накладної", ex);
    }
}

    

    public async Task<IEnumerable<OutputInvoice>> GetOutputInvoices(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<OutputInvoice>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Видаткових накладних", ex);
        }
    }
    public async Task<OutputInvoice> GetOutputInvoiceByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<OutputInvoice>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Видаткової накладної з ID {id}", ex);
        }
    }
    
    
    public async Task<IEnumerable<OutputInvoice>> SearchOutputInvoices(
        int? id,
        string? invoiceNumber,
        DateTime? shipmentDate,
        string? vehicleNumber,
        int? supplierId,
        int? productId,
        string productCategory,
        int? productWeight,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size, 
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<OutputInvoice>();
            
            if (id.HasValue)
                query = query.Where(ii => ii.Id == id.Value);

            if (!string.IsNullOrEmpty(invoiceNumber))
                query = query.Where(ii => ii.InvoiceNumber == invoiceNumber);

            if (shipmentDate.HasValue)
                query = query.Where(ii => ii.ShipmentDate.Date == shipmentDate.Value.Date);

            if (!string.IsNullOrEmpty(vehicleNumber))
                query = query.Where(ii => ii.VehicleNumber == vehicleNumber);
            
            if (supplierId.HasValue)
                query = query.Where(ii => ii.SupplierId == supplierId.Value);

            if (productId.HasValue)
                query = query.Where(ii => ii.ProductId == productId.Value);
            
            if (!string.IsNullOrEmpty(productCategory))
                query = query.Where(ii => ii.ProductCategory == productCategory);
            
            if (productWeight.HasValue)
                query = query.Where(ii => ii.ProductWeight == productWeight.Value);

            if (createdById.HasValue)
                query = query.Where(ii => ii.CreatedById == createdById.Value);

            if (removedAt.HasValue)
                query = query.Where(ii => ii.RemovedAt == removedAt.Value);
            
            return await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Видаткових накладних", ex);
        }
    }
    public async Task<OutputInvoice> UpdateOutputInvoiceAsync(OutputInvoice outputInvoice, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            outputInvoice.ModifiedAt = DateTime.UtcNow;
            outputInvoice.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(outputInvoice, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Видаткової накладної з ID  {outputInvoice.Id}", ex);
        }
    }
    
    public async Task<OutputInvoice> SoftDeleteOutputInvoiceAsync(OutputInvoice outputInvoice, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            outputInvoice.RemovedAt = DateTime.UtcNow;
            outputInvoice.RemovedById = removedById;
            
            return await _repository.UpdateAsync(outputInvoice, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Видаткової накладної з ID  {outputInvoice.Id}", ex);
        }
    }
    
    public async Task<OutputInvoice> RestoreRemovedOutputInvoiceAsync(OutputInvoice outputInvoice, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            outputInvoice.RemovedAt = null;
            outputInvoice.RestoredAt = DateTime.UtcNow;
            outputInvoice.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(outputInvoice, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Видаткової накладної з ID  {outputInvoice.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteOutputInvoiceAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _repository.GetByIdAsync<OutputInvoice>(id, cancellationToken);
            if (supplier != null)
            {
                await _repository.DeleteAsync<OutputInvoice>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Видаткової накладної з ID {id}", ex);
        }
    }
}