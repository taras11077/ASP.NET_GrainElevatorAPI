using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class WarehouseUnitService: IWarehouseUnitService
{
    private readonly IRepository _repository;
    private readonly IInvoiceRegisterService _invoiceRegisterService;

    public WarehouseUnitService(IRepository repository, IInvoiceRegisterService invoiceRegisterService)
    {
        _repository = repository;
        _invoiceRegisterService = invoiceRegisterService;
    }
    
    public async Task<WarehouseUnit> WarehouseTransferAsync(int invoiceRegisterId, int createdById)
    {
        try
        {
            var register = await _invoiceRegisterService.GetRegisterByIdAsync(invoiceRegisterId);
            if (register == null)
            {
                throw new Exception($"Реєстр з ID {invoiceRegisterId} не знайдено.");
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
                    CreatedById = createdById,
                    ProductCategories = new List<WarehouseProductCategory>
                    {
                        new WarehouseProductCategory("Кондиційна продукція") { Value = register.AccWeightReg },
                        new WarehouseProductCategory("Відходи") { Value = register.WasteReg }
                    }
                };
        
                await _repository.AddAsync(warehouseUnit);
            }
            else
            {
                // оновлення існуючого WarehouseUnit новими даними з Register
                await UpdateWarehouseUnitWithRegisterData(warehouseUnit, register);
            }

            return warehouseUnit;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при обробці операції переміщення до складу продукції Реєстру з ID {invoiceRegisterId}", ex);
        }
    }

    
    public async Task UpdateWarehouseUnitWithRegisterData(WarehouseUnit warehouseUnit, InvoiceRegister register)
    {
        // пошук категорії для кондиційної продукції
        var conditionedProduct = warehouseUnit.ProductCategories
            .FirstOrDefault(pc => pc.Title == "Кондиційна продукція");
        if (conditionedProduct == null)
        {
            conditionedProduct = new WarehouseProductCategory("Кондиційна продукція") 
            { 
                Value = register.AccWeightReg, 
                WarehouseUnitId = warehouseUnit.Id 
            };
            warehouseUnit.ProductCategories.Add(conditionedProduct);
        }
        else
        {
            conditionedProduct.Value = register.AccWeightReg;
        }

        // пошук категорії для відходів
        var wasteProduct = warehouseUnit.ProductCategories
            .FirstOrDefault(pc => pc.Title == "Відходи");
        if (wasteProduct == null)
        {
            wasteProduct = new WarehouseProductCategory("Відходи") 
            { 
                Value = register.WasteReg, 
                WarehouseUnitId = warehouseUnit.Id 
            };
            warehouseUnit.ProductCategories.Add(wasteProduct);
        }
        else
        {
            wasteProduct.Value = register.WasteReg;
        }

        await _repository.UpdateAsync(warehouseUnit);
    }

    
    public IQueryable<WarehouseUnit> GetPagedWarehouseUnits(int page, int size)
    {
        try
        {
            return _repository.GetAll<WarehouseUnit>()
                .Skip((page - 1) * size)
                .Take(size);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку Cкладських одиниць", ex);
        }
    }

    public async Task<WarehouseUnit> GetWarehouseUnitByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync<WarehouseUnit>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Cкладської одиниці з ID {id}", ex);
        }
    }

    public IEnumerable<WarehouseUnit> SearchWarehouseUnits(int? id,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size)
    {
        try
        {
            var query = GetPagedWarehouseUnits(page, size)
                .AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(wu => wu.Id == id);
            }
            
            if (supplierId.HasValue)
                query = query.Where(wu => wu.SupplierId == supplierId.Value);

            if (productId.HasValue)
                query = query.Where(wu => wu.ProductId == productId.Value);

            
            if (createdById.HasValue)
                query = query.Where(wu => wu.CreatedById == createdById.Value);

            if (removedAt.HasValue)
                query = query.Where(wu => wu.RemovedAt == removedAt.Value);
            
            return query.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при пошуку Cкладських одиниць за параметрами", ex);
        }
    }
    
    public async Task<WarehouseUnit> UpdateWarehouseUnitAsync(WarehouseUnit warehouseUnit, int modifiedById)
    {
        try
        {
            warehouseUnit.ModifiedAt = DateTime.UtcNow;
            warehouseUnit.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(warehouseUnit);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
        }
    }

    public async Task<WarehouseUnit> SoftDeleteWarehouseUnitAsync(WarehouseUnit warehouseUnit, int removedById)
    {
        try
        {
            warehouseUnit.RemovedAt = DateTime.UtcNow;
            warehouseUnit.RemovedById = removedById;
            
            return await _repository.UpdateAsync(warehouseUnit);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
        }
    }
    
    public async Task<WarehouseUnit> RestoreRemovedWarehouseUnitAsync(WarehouseUnit warehouseUnit, int restoredById)
    {
        try
        {
            warehouseUnit.RemovedAt = null;
            warehouseUnit.RestoredAt = DateTime.UtcNow;
            warehouseUnit.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(warehouseUnit);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при відновленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteWarehouseUnitAsync(int id)
    {
        try
        {
            var warehouseUnit = await _repository.GetByIdAsync<WarehouseUnit>(id);
            if (warehouseUnit != null)
            {
                await _repository.DeleteAsync<WarehouseUnit>(id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Cкладської одиниці з ID {id}", ex);
        }
    }
}