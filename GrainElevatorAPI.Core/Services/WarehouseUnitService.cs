using System.ComponentModel.Design;
using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class WarehouseUnitService: IWarehouseUnitService
{
    private readonly IRepository _repository;
    private IWarehouseUnitService _iWarehouseUnitServiceImplementation;

    public WarehouseUnitService(IRepository repository)
    {
        _repository = repository;
    }
    
   public async Task<WarehouseUnit> CreateWarehouseUnitAsync(
    string supplierTitle,
    string productTitle,
    int employeeId,
    CancellationToken cancellationToken)
{
    try
    {
        // Отримання постачальника
        var supplier = await _repository.GetAll<Supplier>()
            .FirstOrDefaultAsync(s => s.Title == supplierTitle, cancellationToken);
        if (supplier == null)
        {
            throw new ArgumentException($"Постачальник з назвою '{supplierTitle}' не знайдено.", nameof(supplierTitle));
        }

        // Отримання продукту
        var product = await _repository.GetAll<Product>()
            .FirstOrDefaultAsync(p => p.Title == productTitle, cancellationToken);
        if (product == null)
        {
            throw new ArgumentException($"Продукт з назвою '{productTitle}' не знайдено.", nameof(productTitle));
        }

        // Перевірка наявності WarehouseUnit із заданими SupplierId і ProductId
        var existingWarehouseUnit = await _repository.GetAll<WarehouseUnit>()
            .FirstOrDefaultAsync(
                w => w.SupplierId == supplier.Id && w.ProductId == product.Id,
                cancellationToken);

        if (existingWarehouseUnit != null)
        {
            // Повертаємо наявний складський юніт
            return existingWarehouseUnit;
        }

        // Створення нового складського юніта
        var newWarehouseUnit = new WarehouseUnit
        {
            SupplierId = supplier.Id,
            ProductId = product.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedById = employeeId,
            ProductCategories = new List<WarehouseProductCategory>
            {
                new WarehouseProductCategory { Title = "Кондиційна продукція", Value = 0 },
                new WarehouseProductCategory { Title = "Відходи", Value = 0 }
            }
        };

        await _repository.AddAsync(newWarehouseUnit, cancellationToken);
        return newWarehouseUnit;
    }
    catch (ArgumentException ex)
    {
        throw new InvalidOperationException($"Некоректні дані: {ex.Message}", ex);
    }
    catch (Exception ex)
    {
        throw new Exception("Помилка сервісу при створенні Складського юніта", ex);
    }
}
   
    public async Task<WarehouseUnit> WarehouseTransferAsync(InvoiceRegister register, int employeeId, CancellationToken cancellationToken)
    {
        try
        {
            if (register == null)
            {
                throw new Exception($"Реєстр не знайдено.");
            }

            // перевірка наявності WarehouseUnit із заданими SupplierId і ProductId
            var warehouseUnit = await _repository.GetAll<WarehouseUnit>()
                .FirstOrDefaultAsync(w => w.SupplierId == register.SupplierId && w.ProductId == register.ProductId);

            if (warehouseUnit == null)
            {
                warehouseUnit = new WarehouseUnit()
                {
                    SupplierId = register.SupplierId,
                    ProductId = register.ProductId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = employeeId,
                    ProductCategories = new List<WarehouseProductCategory>
                    {
                        new WarehouseProductCategory() {Title = "Кондиційна продукція",  Value = register.AccWeightReg },
                        new WarehouseProductCategory() {Title = "Відходи", Value = register.WasteReg }
                    }
                };
        
                await _repository.AddAsync(warehouseUnit, cancellationToken);
            }
            else
            {
                // оновлення існуючого WarehouseUnit новими даними з Register
                await AddingRegisterDataToWarehouseUnitAsync(warehouseUnit, register, employeeId, cancellationToken);
            }

            return warehouseUnit;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при обробці операції переміщення до складу продукції Реєстру з ID {register.Id}", ex);
        }
    }
    
    private async Task AddingRegisterDataToWarehouseUnitAsync(WarehouseUnit warehouseUnit, InvoiceRegister register, int employeeId, CancellationToken cancellationToken)
    {
        // пошук категорії для кондиційної продукції
        var conditionedProduct = warehouseUnit.ProductCategories
            .FirstOrDefault(pc => pc.Title == "Кондиційна продукція");
        
        if (conditionedProduct == null)
        {
            conditionedProduct = new WarehouseProductCategory() 
            { 
                Title = "Кондиційна продукція",
                Value = register.AccWeightReg, 
                WarehouseUnitId = warehouseUnit.Id, 
                CreatedById = employeeId,
            };
            warehouseUnit.ProductCategories.Add(conditionedProduct);
            warehouseUnit.ModifiedById = employeeId;
        }
        else
        {
            conditionedProduct.Value += register.AccWeightReg;
            
            conditionedProduct.ModifiedById = employeeId;
            warehouseUnit.ModifiedById = employeeId;
        }

        // пошук категорії для відходів
        var wasteProduct = warehouseUnit.ProductCategories
            .FirstOrDefault(pc => pc.Title == "Відходи");
        if (wasteProduct == null)
        {
            wasteProduct = new WarehouseProductCategory() 
            { 
                Title = "Відходи",
                Value = register.WasteReg, 
                WarehouseUnitId = warehouseUnit.Id,
                CreatedById = employeeId,
            };
            warehouseUnit.ProductCategories.Add(wasteProduct);
        }
        else
        {
            wasteProduct.Value += register.WasteReg;
            
            wasteProduct.ModifiedById = employeeId;
            warehouseUnit.ModifiedById = employeeId;
        }

        await _repository.UpdateAsync(warehouseUnit, cancellationToken);
    }
    
    public async Task DeletingRegisterDataFromWarehouseUnitAsync(InvoiceRegister register, int modifiedById, CancellationToken cancellationToken)
    {
        // перевірка наявності WarehouseUnit із заданими SupplierId і ProductId
        var warehouseUnit = await _repository.GetAll<WarehouseUnit>()
            .FirstOrDefaultAsync(w => w.SupplierId == register.SupplierId && w.ProductId == register.ProductId);
        
        // пошук категорії для кондиційної продукції
        var conditionedProduct = warehouseUnit.ProductCategories
            .FirstOrDefault(pc => pc.Title == "Кондиційна продукція");
  
        conditionedProduct.Value -= register.AccWeightReg;
        

        // пошук категорії для відходів
        var wasteProduct = warehouseUnit.ProductCategories
            .FirstOrDefault(pc => pc.Title == "Відходи");

        wasteProduct.Value -= register.WasteReg;

        warehouseUnit.ModifiedById = modifiedById;
        
        await _repository.UpdateAsync(warehouseUnit, cancellationToken);
    }
    
    public async Task DeletingOutputInvoiceDataFromWarehouseUnitAsync(OutputInvoice invoice, int modifiedById, CancellationToken cancellationToken)
    {
        // перевірка наявності WarehouseUnit із заданими SupplierId і ProductId
        var warehouseUnit = await _repository.GetAll<WarehouseUnit>()
            .FirstOrDefaultAsync(w => w.SupplierId == invoice.SupplierId && w.ProductId == invoice.ProductId);
        
        if (warehouseUnit == null)
        {
            throw new KeyNotFoundException($"Складський юніт не знайдено.");
        }
        
        // оновлення значень Категорій продукції Складського юніта
        foreach (var productCategory in warehouseUnit.ProductCategories)
        {
            if (productCategory.Title == invoice.ProductCategory)
                productCategory.Value += invoice.ProductWeight;
        }
        
        warehouseUnit.ModifiedById = modifiedById;
        
        await _repository.UpdateAsync(warehouseUnit, cancellationToken);
    }
    
    public async Task<IEnumerable<WarehouseUnit>> GetPagedWarehouseUnitsAsync(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<WarehouseUnit>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Cкладських одиниць", ex);
        }
    }

    public async Task<WarehouseUnit> GetWarehouseUnitByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<WarehouseUnit>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Cкладської одиниці з ID {id}", ex);
        }
    }
    
    public async Task<(IEnumerable<WarehouseUnit>, int)> SearchWarehouseUnitsAsync(
        string? supplierTitle = null,
        string? productTitle = null,
        string? createdByName = null,
        int page = 1,
        int size = 10,
        string? sortField = null,
        string? sortOrder = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _repository.GetAll<WarehouseUnit>()
                .Include(wu => wu.ProductCategories)
                .Include(wu => wu.Supplier)
                .Include(wu => wu.Product)
                .Include(wu => wu.CreatedBy)
                .Where(wu => wu.RemovedAt == null);

            // Виклик методу фільтрації
            query = ApplyFilters(query, supplierTitle, productTitle, createdByName);

            // Виклик методу сортування
            query = ApplySorting(query, sortField, sortOrder);

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredUnits = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredUnits, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Cкладських одиниць за параметрами", ex);
        }
    }
    
     private IQueryable<WarehouseUnit> ApplyFilters(
        IQueryable<WarehouseUnit> query,
        string? supplierTitle,
        string? productTitle,
        string? createdByName)
    {

        if (!string.IsNullOrEmpty(supplierTitle))
        {
            query = query.Where(wu => wu.Supplier.Title == supplierTitle);
        }
        if (!string.IsNullOrEmpty(productTitle))
        {
            query = query.Where(wu => wu.Product.Title == productTitle);
        }
        if (!string.IsNullOrEmpty(createdByName))
        {
            query = query.Where(wu => wu.CreatedBy.LastName == createdByName);
        }
        
        return query;
    }

    
    private IQueryable<WarehouseUnit> ApplySorting(
        IQueryable<WarehouseUnit> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "productTitle" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.Product.Title)
                : query.OrderByDescending(reg => reg.Product.Title),
            "supplierTitle" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.Supplier.Title)
                : query.OrderByDescending(reg => reg.Supplier.Title),
            "createdByName" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.CreatedBy.LastName)
                : query.OrderByDescending(reg => reg.CreatedBy.LastName),
            _ => query // Якщо поле не визначене
        };
    }
    
    public async Task<WarehouseUnit> UpdateWarehouseUnitAsync(WarehouseUnit warehouseUnit, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            warehouseUnit.ModifiedAt = DateTime.UtcNow;
            warehouseUnit.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(warehouseUnit, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
        }
    }

    public async Task<WarehouseUnit> SoftDeleteWarehouseUnitAsync(WarehouseUnit warehouseUnit, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            bool empty = true;
            foreach (var productCategory in warehouseUnit.ProductCategories)
            {
                if(productCategory.Value != 0)
                    empty = false;
            }

            if (!empty)
                throw new CheckoutException($"Дозволено видалення тільки порожнього Складського юніта");
            
            warehouseUnit.RemovedAt = DateTime.UtcNow;
            warehouseUnit.RemovedById = removedById;
            var deletedWarehouseUnit = await _repository.UpdateAsync(warehouseUnit, cancellationToken);
            
            return deletedWarehouseUnit;
        }
        catch (CheckoutException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
        }
    }
    
    public async Task<WarehouseUnit> RestoreRemovedWarehouseUnitAsync(WarehouseUnit warehouseUnit, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            warehouseUnit.RemovedAt = null;
            warehouseUnit.RestoredAt = DateTime.UtcNow;
            warehouseUnit.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(warehouseUnit, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteWarehouseUnitAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var warehouseUnit = await _repository.GetByIdAsync<WarehouseUnit>(id, cancellationToken);
            if (warehouseUnit != null)
            {
                await _repository.DeleteAsync<WarehouseUnit>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Cкладської одиниці з ID {id}", ex);
        }
    }
}