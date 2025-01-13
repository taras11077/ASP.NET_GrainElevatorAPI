using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
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
    
    
    public async Task<InputInvoice> CreateInputInvoiceAsync(
        string invoiceNumber,
        DateTime arrivalDate,
        string supplierTitle, 
        string productTitle, 
        int physicalWeight,
        string vehicleNumber,
        int createdById, 
        CancellationToken cancellationToken)
    {
        try
        {
            // початок транзакції
            await _repository.BeginTransactionAsync(cancellationToken);
            
            var supplier = await _repository.GetAll<Supplier>()
                .FirstOrDefaultAsync(s => s.Title == supplierTitle, cancellationToken);
        
            if (supplier == null)
            {
                supplier = new Supplier { Title = supplierTitle };
                await _repository.AddAsync(supplier, cancellationToken);
            }
            
            var product = await _repository.GetAll<Product>()
                .FirstOrDefaultAsync(p => p.Title == productTitle, cancellationToken);
        
            if (product == null)
            {
                product = new Product { Title = productTitle };
                await _repository.AddAsync(product, cancellationToken);
            }
            
            var inputInvoice = new InputInvoice
            {
                InvoiceNumber = invoiceNumber,
                ArrivalDate = arrivalDate,
                CreatedAt = DateTime.UtcNow,
                CreatedById = createdById,
                SupplierId = supplier.Id,
                ProductId = product.Id,
                PhysicalWeight = physicalWeight,
                VehicleNumber = vehicleNumber,
                Supplier = supplier,
                Product = product
            };

            var addedInvoice = await _repository.AddAsync(inputInvoice, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            
            // фіксація транзакції
            await _repository.CommitTransactionAsync(cancellationToken);

            return addedInvoice;
        }
        catch (Exception ex)
        {
            // відкат транзакції в разі помилки
            await _repository.RollbackTransactionAsync(cancellationToken);
            throw new Exception("Помилка сервісу при додаванні Прибуткової накладної", ex);
        }
    }

    public async Task<(IEnumerable<InputInvoice>, int)> GetInputInvoicesAsync(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<InputInvoice>()
                .Include(ii => ii.Supplier)
                .Include(ii => ii.Product)
                .Include(ii => ii.CreatedBy);
            
            int totalCount = await query.CountAsync(cancellationToken);
            
            var invoices = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
            
            return (invoices, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Прибуткових накладних", ex);
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
            throw new Exception($"Помилка сервісу при отриманні Прибуткової накладної з ID {id}", ex);
        }
    }
    
    public async Task<(IEnumerable<InputInvoice>, int)> SearchInputInvoices(
    int? id, 
    string? invoiceNumber = null,
    DateTime? arrivalDate = null,
    string? vehicleNumber = null,
    int? physicalWeight = null,
    string? supplierTitle = null,
    string? productTitle = null,
    string? createdByName = null,
    DateTime? removedAt = null,
    int page = 1,
    int size = 10,
    string? sortField = null,
    string? sortOrder = null,
    CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _repository.GetAll<InputInvoice>()
                .Include(ii => ii.Supplier) 
                .Include(ii => ii.Product)  
                .Include(ii => ii.CreatedBy)
                .Where(ii => ii.RemovedAt == null);

            // Фільтрація
            query = ApplyFilters(query, 
                invoiceNumber, 
                arrivalDate, 
                vehicleNumber, 
                physicalWeight, 
                supplierTitle, 
                productTitle, 
                createdByName, 
                removedAt);

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
            throw new Exception("Помилка сервісу при пошуку Прибуткових накладних", ex);
        }
    }
    
    
     private IQueryable<InputInvoice> ApplyFilters(
        IQueryable<InputInvoice> query,
        string? invoiceNumber,
        DateTime? arrivalDate,
        string? vehicleNumber,
        int? physicalWeight,
        string? supplierTitle,
        string? productTitle,
        string? createdByName,
        DateTime? removedAt)
    {
        if (!string.IsNullOrEmpty(invoiceNumber))
            query = query.Where(ii => ii.InvoiceNumber.Contains(invoiceNumber));

        if (arrivalDate.HasValue)
            query = query.Where(ii => ii.ArrivalDate.Date == arrivalDate.Value.Date);

        if (!string.IsNullOrEmpty(vehicleNumber))
            query = query.Where(ii => ii.VehicleNumber.Contains(vehicleNumber));

        if (physicalWeight.HasValue)
            query = query.Where(ii => ii.PhysicalWeight == physicalWeight.Value);

        if (!string.IsNullOrEmpty(supplierTitle))
            query = query.Where(ii => ii.Supplier.Title.Contains(supplierTitle));

        if (!string.IsNullOrEmpty(productTitle))
            query = query.Where(ii => ii.Product.Title.Contains(productTitle));

        if (!string.IsNullOrEmpty(createdByName))
            query = query.Where(ii => ii.CreatedBy.LastName.Contains(createdByName));

        if (removedAt.HasValue)
            query = query.Where(ii => ii.RemovedAt.HasValue && ii.RemovedAt.Value.Date == removedAt.Value.Date);
        
        return query;
    }

    
    private IQueryable<InputInvoice> ApplySorting(
        IQueryable<InputInvoice> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "invoiceNumber" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.InvoiceNumber) 
                : query.OrderByDescending(ii => ii.InvoiceNumber),
            "arrivalDate" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.ArrivalDate) 
                : query.OrderByDescending(ii => ii.ArrivalDate),
            "physicalWeight" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.PhysicalWeight) 
                : query.OrderByDescending(ii => ii.PhysicalWeight),
            "vehicleNumber" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.VehicleNumber) 
                : query.OrderByDescending(ii => ii.VehicleNumber),
            "productTitle" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.Product.Title) 
                : query.OrderByDescending(ii => ii.Product.Title),
            "supplierTitle" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.Supplier.Title) 
                : query.OrderByDescending(ii => ii.Supplier.Title),
            "createdByName" => sortOrder == "asc" 
                ? query.OrderBy(ii => ii.CreatedBy.LastName) 
                : query.OrderByDescending(ii => ii.CreatedBy.LastName),
            _ => query // без сортування якщо поле не вказано
        };
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
            throw new Exception($"Помилка сервісу при оновленні Прибуткової накладної з ID  {inputInvoice.Id}", ex);
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
            throw new Exception($"Помилка сервісу при видаленні Прибуткової накладної з ID  {inputInvoice.Id}", ex);
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
            throw new Exception($"Помилка сервісу при відновленні Прибуткової накладної з ID  {inputInvoice.Id}", ex);
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
            throw new Exception($"Помилка сервісу при видаленні Прибуткової накладної з ID {id}", ex);
        }
    }
    
    
    public async Task<(Dictionary<string, int> BySupplier, Dictionary<string, int> ByProduct)> 
        GetTotalPhysicalWeightBySupplierAndProductAsync(CancellationToken cancellationToken)
    {
        // Отримуємо всі накладні
        var invoices = await _repository.GetAll<InputInvoice>()
            .Include(ii => ii.Supplier)
            .Include(ii => ii.Product)
            .ToListAsync(cancellationToken);

        // Групуємо за постачальниками
        var bySupplier = invoices
            .GroupBy(invoice => invoice.Supplier)
            .ToDictionary(
                group => group.Key.Title, // Ім'я постачальника
                group => group.Sum(invoice => invoice.PhysicalWeight) // Сума фізичної ваги
            );

        // Групуємо за продуктами
        var byProduct = invoices
            .GroupBy(invoice => invoice.Product)
            .ToDictionary(
                group => group.Key.Title, // Назва продукту
                group => group.Sum(invoice => invoice.PhysicalWeight) // Сума фізичної ваги
            );

        return (BySupplier: bySupplier, ByProduct: byProduct);
    }
    
    
    public async Task<Dictionary<DateTime, Dictionary<string, int>>> GetProductArrivalsAsync(CancellationToken cancellationToken)
    {
        var invoices = await _repository.GetAll<InputInvoice>()
            .Include(ii => ii.Product)
            .ToListAsync(cancellationToken);

        return invoices
            .GroupBy(i => i.ArrivalDate.Date)
            .ToDictionary(
                group => group.Key,
                group => group
                    .GroupBy(i => i.Product.Title)
                    .ToDictionary(
                        productGroup => productGroup.Key,
                        productGroup => productGroup.Sum(i => i.PhysicalWeight)
                    )
            );
    }

}