using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class InvoiceRegisterService : IInvoiceRegisterService
{
    private readonly IRepository _repository;
    private readonly IWarehouseUnitService _warehouseUnitService;
    private readonly IRegisterCalculator _calculator;
    
    public InvoiceRegisterService(IRepository repository, IRegisterCalculator calculator, IWarehouseUnitService warehouseUnitService)
    {
        _repository = repository;
        _calculator = calculator;
        _warehouseUnitService = warehouseUnitService;
    }

    public async Task<InvoiceRegister> CreateRegisterAsync(
        string registerNumber,
        int supplierId, 
        int productId, 
        double weedImpurityBase, 
        double moistureBase, 
        IEnumerable<int> laboratoryCardIds, 
        int createdById, 
        CancellationToken cancellationToken)
    {
        try
        {
            // початок транзакції
            await _repository.BeginTransactionAsync(cancellationToken);
            
            // створення Реєстру (доробка продукції)
            var laboratoryCards = _repository.GetAll<LaboratoryCard>()
                .Where(r => laboratoryCardIds.Contains(r.Id))
                .ToList();
            
            var arrivalDate = laboratoryCards.First().InputInvoice.ArrivalDate;

            var register = new InvoiceRegister
            {
                RegisterNumber = registerNumber,
                ArrivalDate = arrivalDate,
                WeedImpurityBase = weedImpurityBase,
                MoistureBase = moistureBase,
                SupplierId = supplierId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = createdById,
                
                PhysicalWeightReg = 0,
                ShrinkageReg = 0,
                WasteReg = 0,
                AccWeightReg = 0,
                QuantitiesDryingReg = 0,   
            };

            var calculatedRegister = MapLabCardsToProductionBatches(laboratoryCards, register, createdById);

            await _repository.AddAsync(register, cancellationToken);
            
            // створення або оновлення складського юніта (переміщення продукції Реєстру на Склад)
            await _warehouseUnitService.WarehouseTransferAsync(calculatedRegister, createdById, cancellationToken);
            
            // фіксація транзакції
            await _repository.CommitTransactionAsync(cancellationToken);
            
            return register;
        }
        catch (Exception ex)
        {
            // відкат транзакції в разі помилки
            await _repository.RollbackTransactionAsync(cancellationToken);
            throw new Exception("Помилка сервісу під час створення Реєстру", ex);
        }
        
    }

    private InvoiceRegister MapLabCardsToProductionBatches(List<LaboratoryCard> laboratoryCards, InvoiceRegister register, int employeeId)
    {
        foreach (var labCard in laboratoryCards)
        {
            var productionBatch = new ProductionBatch
            {
                LaboratoryCardId = labCard.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedById = employeeId
            };
            register = (InvoiceRegister)_calculator.CalcProductionBatch(labCard.InputInvoice, labCard, register, productionBatch);
        }
        
        return register;
    }
    
    // // допоміжний метод для генерації номера реєстру
    // private string GenerateRegisterNumber()
    // {
    //     return $"№{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    // }
    
    public async Task<InvoiceRegister> UpdateRegisterAsync(int id, 
        string? registerNumber, 
        double? weedImpurityBase, 
        double? moistureBase, 
        List<int>? laboratoryCardIds, 
        int modifiedById, 
        CancellationToken cancellationToken)
    {
        try
        {
            var invoiceRegisterDb = await _repository.GetByIdAsync<InvoiceRegister>(id, cancellationToken);
            
            invoiceRegisterDb.RegisterNumber = registerNumber ?? invoiceRegisterDb.RegisterNumber;
            invoiceRegisterDb.WeedImpurityBase = weedImpurityBase ?? invoiceRegisterDb.WeedImpurityBase;
            invoiceRegisterDb.MoistureBase = moistureBase ?? invoiceRegisterDb.MoistureBase;
            
            if (laboratoryCardIds != null && laboratoryCardIds.Any())
            {
                List<LaboratoryCard> laboratoryCards = await _repository.GetAll<LaboratoryCard>()
                    .Where(lc => laboratoryCardIds.Contains(lc.Id))
                    .ToListAsync(cancellationToken);
                invoiceRegisterDb = MapLabCardsToProductionBatches(laboratoryCards, invoiceRegisterDb, modifiedById);
            }
            
            return await _repository.UpdateAsync(invoiceRegisterDb, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу під час оновлення Реєстру з ID  {id}", ex);
        }
    }

    public async Task<InvoiceRegister> GetRegisterByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<InvoiceRegister>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Реєстру з ID {id}", ex);
        }
    }
    
    public async Task<IEnumerable<InvoiceRegister>> GetRegistersAsync(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<InvoiceRegister>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Реєстрів", ex);
        }
    }

    
     public async Task<IEnumerable<InvoiceRegister>> SearchRegistersAsync(int? id,
        string? registerNumber,
        DateTime? arrivalDate,
        int? supplierId,
        int? productId,
        int? physicalWeightReg,
        int? shrinkageReg,
        int? wasteReg,
        int? accWeightReg,
        double? weedImpurityBase,
        double? moistureBase,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size,
        CancellationToken cancellationToken)
    {
        try
        
        {
            var query = _repository.GetAll<InvoiceRegister>()
                .Skip((page - 1) * size)
                .Take(size);

            if (id.HasValue)
            {
                query = query.Where(r => r.Id == id);
            }
            
            if (!string.IsNullOrEmpty(registerNumber))
            {
                query = query.Where(r => r.RegisterNumber == registerNumber);
            }

            if (arrivalDate.HasValue)
            {
                query = query.Where(r => r.ArrivalDate.Date == arrivalDate.Value.Date);
            }
            
            if (supplierId.HasValue)
                query = query.Where(r => r.SupplierId == supplierId.Value);

            if (productId.HasValue)
                query = query.Where(r => r.ProductId == productId.Value);
            
            
            if (weedImpurityBase.HasValue)
                query = query.Where(r => r.WeedImpurityBase == weedImpurityBase.Value);

            if (moistureBase.HasValue)
                query = query.Where(r => r.MoistureBase == moistureBase.Value);
           
            if (physicalWeightReg.HasValue)
                query = query.Where(r => r.PhysicalWeightReg == physicalWeightReg.Value);
            
            if (accWeightReg.HasValue)
                query = query.Where(r => r.AccWeightReg == accWeightReg.Value);
            
            if (shrinkageReg.HasValue)
                query = query.Where(r => r.ShrinkageReg == shrinkageReg.Value);
            
            if (wasteReg.HasValue)
                query = query.Where(r => r.WasteReg == wasteReg.Value);
            
            if (createdById.HasValue)
                query = query.Where(r => r.CreatedById == createdById.Value);

            if (removedAt.HasValue)
                query = query.Where(r => r.RemovedAt == removedAt.Value);
            
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Реєстру за параметрами", ex);
        }
    }
    
    
    
    
    public async Task<InvoiceRegister> SoftDeleteRegisterAsync(InvoiceRegister invoiceRegister, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            invoiceRegister.RemovedAt = DateTime.UtcNow;
            invoiceRegister.RemovedById = removedById;
            
            return await _repository.UpdateAsync(invoiceRegister, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу під час видалення Реєстру з ID  {invoiceRegister.Id}", ex);
        }
    }
    
    public async Task<InvoiceRegister> RestoreRemovedRegisterAsync(InvoiceRegister invoiceRegister, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            invoiceRegister.RemovedAt = null;
            invoiceRegister.RestoredAt = DateTime.UtcNow;
            invoiceRegister.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(invoiceRegister, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Реєстру з ID  {invoiceRegister.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteRegisterAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var register = await _repository.GetByIdAsync<InvoiceRegister>(id, cancellationToken);
            if (register != null)
            {
                await _repository.DeleteAsync<InvoiceRegister>(id, cancellationToken);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при hard-видаленні Реєстру з ID {id}", ex);
        }
    }
}