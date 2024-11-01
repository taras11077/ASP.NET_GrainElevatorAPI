using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class WarehouseUnitService: IWarehouseUnitService
{
    private readonly IRepository _repository;

    public WarehouseUnitService(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<WarehouseUnit> WarehouseTransferAsync(InvoiceRegister register, int createdById, CancellationToken cancellationToken)
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
                    CreatedById = createdById,
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
                await UpdateWarehouseUnitWithRegisterData(warehouseUnit, register, cancellationToken);
            }

            return warehouseUnit;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при обробці операції переміщення до складу продукції Реєстру з ID {register.Id}", ex);
        }
    }

    
    private async Task UpdateWarehouseUnitWithRegisterData(WarehouseUnit warehouseUnit, InvoiceRegister register, CancellationToken cancellationToken)
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
            wasteProduct = new WarehouseProductCategory() 
            { 
                Title = "Відходи",
                Value = register.WasteReg, 
                WarehouseUnitId = warehouseUnit.Id 
            };
            warehouseUnit.ProductCategories.Add(wasteProduct);
        }
        else
        {
            wasteProduct.Value = register.WasteReg;
        }

        await _repository.UpdateAsync(warehouseUnit, cancellationToken);
    }

    
    public async Task<IEnumerable<WarehouseUnit>> GetPagedWarehouseUnits(int page, int size, CancellationToken cancellationToken)
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

    
    
    public async Task<IEnumerable<WarehouseUnit>> SearchWarehouseUnits(int? id,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size, 
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<WarehouseUnit>();

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
            
            return await query .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Cкладських одиниць за параметрами", ex);
        }
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
            warehouseUnit.RemovedAt = DateTime.UtcNow;
            warehouseUnit.RemovedById = removedById;
            
            return await _repository.UpdateAsync(warehouseUnit, cancellationToken);
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