using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using ArgumentNullException = System.ArgumentNullException;

namespace GrainElevatorAPI.Core.Services;

public class WarehouseUnitService: IWarehouseUnitService
{
    private readonly IRepository _repository;

    public WarehouseUnitService(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<WarehouseUnit> WarehouseTransferAsync(InvoiceRegister register, int createdById)
    {
        try
        {
            if (register == null)
            {
                throw new ArgumentNullException($"Реєстр не знайдено.");
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

            await _repository.SaveChangesAsync();
            return warehouseUnit;
        }
        catch(ArgumentNullException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при обробці операції переміщення до складу продукції Реєстру з ID {register.Id}", ex);
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
            throw new Exception("Помилка сервісу при отриманні списку Cкладських одиниць", ex);
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
            throw new Exception($"Помилка сервісу при отриманні Cкладської одиниці з ID {id}", ex);
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
            throw new Exception("Помилка сервісу при пошуку Cкладських одиниць за параметрами", ex);
        }
    }
    
    public async Task<WarehouseUnit> UpdateWarehouseUnitAsync(WarehouseUnit warehouseUnit, int modifiedById)
    {
        try
        {
            warehouseUnit.ModifiedAt = DateTime.UtcNow;
            warehouseUnit.ModifiedById = modifiedById;
            
            await _repository.UpdateAsync(warehouseUnit);
            await _repository.SaveChangesAsync();
            
            return warehouseUnit;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
        }
    }

    public async Task<WarehouseUnit> SoftDeleteWarehouseUnitAsync(WarehouseUnit warehouseUnit, int removedById)
    {
        try
        {
            warehouseUnit.RemovedAt = DateTime.UtcNow;
            warehouseUnit.RemovedById = removedById;
            
            await _repository.UpdateAsync(warehouseUnit);
            await _repository.SaveChangesAsync();
            
            return warehouseUnit;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
        }
    }
    
    public async Task<WarehouseUnit> RestoreRemovedWarehouseUnitAsync(WarehouseUnit warehouseUnit, int restoredById)
    {
        try
        {
            warehouseUnit.RemovedAt = null;
            warehouseUnit.RestoredAt = DateTime.UtcNow;
            warehouseUnit.RestoreById = restoredById;
            
            await _repository.UpdateAsync(warehouseUnit);
            await _repository.SaveChangesAsync();
            
            return warehouseUnit;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Cкладської одиниці з ID  {warehouseUnit.Id}", ex);
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
                await _repository.SaveChangesAsync();
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