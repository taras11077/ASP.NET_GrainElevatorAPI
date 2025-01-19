using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GrainElevatorAPI.Core.Services;

public class OutputInvoiceService : IOutputInvoiceService
{
    private readonly IRepository _repository;
    private readonly IWarehouseUnitService _warehouseUnitService;
    private readonly ILogger<OutputInvoiceService> _logger;

    public OutputInvoiceService(IRepository repository, ILogger<OutputInvoiceService> logger, IWarehouseUnitService warehouseUnitService)
    {
        _repository = repository;
        _logger = logger;
        _warehouseUnitService = warehouseUnitService;
    }
    
    public async Task<OutputInvoice> CreateOutputInvoiceAsync(
    string invoiceNumber,
    DateTime shipmentDate,
    string vehicleNumber,
    string supplierTitle,
    string productTitle,
    string productCategory,
    int productWeight,
    int createdById, 
    CancellationToken cancellationToken)
{
    await _repository.BeginTransactionAsync(cancellationToken);

    try
    {
        // початок транзакції
        //await _repository.BeginTransactionAsync(cancellationToken);
        
        var supplier = await _repository.GetAll<Supplier>()
            .FirstOrDefaultAsync(s => s.Title == supplierTitle, cancellationToken);
        if (supplier == null)
            throw new KeyNotFoundException($"Постачальника з назвою '{supplierTitle}' не знайдено.");

        var product = await _repository.GetAll<Product>()
            .FirstOrDefaultAsync(p => p.Title == productTitle, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Продукт з назвою '{productTitle}' не знайдено.");
      
        
        var warehouseUnit = await _repository.GetAll<WarehouseUnit>()
            .FirstOrDefaultAsync(w => w.SupplierId == supplier.Id && w.ProductId == product.Id, cancellationToken);
        if (warehouseUnit == null)
            throw new KeyNotFoundException($"Складський юніт із Постачальником {supplierTitle} та Продукцією {productTitle} не знайдено.");
        
        
        var productCategoryEntity = await _repository.GetAll<WarehouseProductCategory>()
            .FirstOrDefaultAsync(pc => pc.Title == productCategory && pc.WarehouseUnitId == warehouseUnit.Id, cancellationToken);
        if (productCategoryEntity == null)
            throw new KeyNotFoundException($"Категорію продукту {productCategory} не знайдено на складі.");
        

        // перевірка доступного залишку продукту
        if (productCategoryEntity.Value == null || productCategoryEntity.Value < productWeight)
        {
            throw new InvalidOperationException(
                $"Недостатньо продукту в категорії '{productCategory}'. " +
                $"Доступно: {productCategoryEntity.Value ?? 0}, потрібно: {productWeight}."
            );
        }

        // зменшення значення категорії продукту
        productCategoryEntity.Value -= productWeight;
        await _repository.UpdateAsync(productCategoryEntity, cancellationToken);

        // створення видаткової накладної
        var outputInvoice = new OutputInvoice
        {
            InvoiceNumber = invoiceNumber,
            ShipmentDate = shipmentDate,
            VehicleNumber = vehicleNumber,
            SupplierId = supplier.Id,
            ProductId = product.Id,
            ProductCategory = productCategory,
            ProductWeight = productWeight,
            WarehouseUnitId = warehouseUnit.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedById = createdById,
        };

        var addedInvoice = await _repository.AddAsync(outputInvoice, cancellationToken);

        // Коміт транзакції
        await _repository.CommitTransactionAsync(cancellationToken);

        return addedInvoice;
    }
    catch
    {
        // Відкат транзакції у випадку відсутності запису
        await _repository.RollbackTransactionAsync(cancellationToken);
        throw;
    }
}
    
    public async Task<IEnumerable<OutputInvoice>> GetOutputInvoices(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Запит починається: отримання видаткових накладних зі сторінкою {Page} та розміром {Size}", page, size);
            var start = DateTime.UtcNow;

            var result = await _repository.GetAll<OutputInvoice>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            var end = DateTime.UtcNow;
            _logger.LogInformation("Запит завершено: отримано {Count} накладних за {Duration} мс", result.Count, (end - start).TotalMilliseconds);

            return result;
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
    
    
    public async Task<(IEnumerable<OutputInvoice>, int)> SearchOutputInvoices(
        string? invoiceNumber = null,
        DateTime? shipmentDate = null,
        string? vehicleNumber = null,
        string? supplierTitle = null,
        string? productTitle = null,
        string? productCategory = null,
        int? productWeight = null,
        string? createdByName = null,
        int page = 1,
        int size = 10,
        string? sortField = null,
        string? sortOrder = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _repository.GetAll<OutputInvoice>()
                .Include(oi => oi.Supplier) 
                .Include(oi => oi.Product)  
                .Include(oi => oi.CreatedBy)
                .Where(oi => oi.RemovedAt == null);

            // Фільтрація
            query = ApplyFilters(query, invoiceNumber, shipmentDate, vehicleNumber, 
                supplierTitle, productTitle, productCategory, productWeight, 
                createdByName);
            
            // Сортування
            query = ApplySorting(query, sortField, sortOrder);
        

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredInvoices = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredInvoices, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Видаткових накладних", ex);
        }
    }
    
    
     private IQueryable<OutputInvoice> ApplyFilters(
        IQueryable<OutputInvoice> query,
        string? invoiceNumber,
        DateTime? shipmentDate,
        string? vehicleNumber,
        string? supplierTitle,
        string? productTitle,
        string? productCategory,
        int? productWeight,
        string? createdByName
)
    {
        if (!string.IsNullOrEmpty(invoiceNumber))
            query = query.Where(oi => oi.InvoiceNumber == invoiceNumber);

        if (shipmentDate.HasValue)
            query = query.Where(oi => oi.ShipmentDate.Date == shipmentDate.Value.Date);

        if (!string.IsNullOrEmpty(vehicleNumber))
            query = query.Where(oi => oi.VehicleNumber == vehicleNumber);
            
        if (!string.IsNullOrEmpty(supplierTitle))
            query = query.Where(oi => oi.Supplier.Title.Contains(supplierTitle));

        if (!string.IsNullOrEmpty(productTitle))
            query = query.Where(oi => oi.Product.Title.Contains(productTitle));
            
        if (!string.IsNullOrEmpty(productCategory))
            query = query.Where(oi => oi.ProductCategory == productCategory);
            
        if (productWeight.HasValue)
            query = query.Where(oi => oi.ProductWeight == productWeight.Value);

        if (!string.IsNullOrEmpty(createdByName))
            query = query.Where(oi => oi.CreatedBy.LastName.Contains(createdByName));

        return query;
    }

    
    private IQueryable<OutputInvoice> ApplySorting(
        IQueryable<OutputInvoice> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "invoiceNumber" => sortOrder == "asc" 
                ? query.OrderBy(oi => oi.InvoiceNumber) 
                : query.OrderByDescending(oi => oi.InvoiceNumber),
            "shipmentDate" => sortOrder == "asc" 
                ? query.OrderBy(oi => oi.ShipmentDate) 
                : query.OrderByDescending(ii => ii.ShipmentDate),
            "vehicleNumber" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.VehicleNumber) 
                : query.OrderByDescending(ii => ii.VehicleNumber),
            "productTitle" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.Product.Title) 
                : query.OrderByDescending(ii => ii.Product.Title),
            "supplierTitle" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.Supplier.Title) 
                : query.OrderByDescending(ii => ii.Supplier.Title),
            "productCategory" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.ProductCategory) 
                : query.OrderByDescending(ii => ii.ProductCategory),
            "productWeight" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.ProductWeight) 
                : query.OrderByDescending(ii => ii.ProductWeight),
            "createdByName" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.CreatedBy.LastName) 
                : query.OrderByDescending(ii => ii.CreatedBy.LastName),
            _ => query // без сортування якщо поле не вказано
        };
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
            
            await _warehouseUnitService.DeletingOutputInvoiceDataFromWarehouseUnitAsync(outputInvoice, removedById, cancellationToken);
            
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