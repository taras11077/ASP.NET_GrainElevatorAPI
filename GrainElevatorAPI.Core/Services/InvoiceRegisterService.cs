using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Calculators.Impl;
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
        int supplierId, 
        int productId, 
        DateTime arrivalDate, 
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
                .Where(lc => laboratoryCardIds.Contains(lc.Id))
                .ToList();

            var register = new InvoiceRegister
            {
                RegisterNumber = GenerateRegisterNumber(),
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
            
            foreach (var labCard in laboratoryCards)
            {
                var productionBatch = new ProductionBatch
                {
                    LaboratoryCardId = labCard.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = createdById
                };
                register = (InvoiceRegister)_calculator.CalcProductionBatch(labCard.InputInvoice, labCard, register, productionBatch);
            }

            await _repository.AddAsync(register, cancellationToken);
            
            // створення або оновлення складського юніта (переміщення продукції Реєстру на Склад)
            await _warehouseUnitService.WarehouseTransferAsync(register, createdById, cancellationToken);
            
            // фіксація транзакції
            await _repository.CommitTransactionAsync(cancellationToken);
            
            return register;
        }
        catch (Exception ex)
        {
            // відкат транзакції в разі помилки
            await _repository.RollbackTransactionAsync(cancellationToken);
            throw new Exception("Помилка при створенні Реєстру", ex);
        }
        
    }
    
    // допоміжний метод для генерації номера реєстрe
    private string GenerateRegisterNumber()
    {
        return $"№{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    public async Task<InvoiceRegister> GetRegisterByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<InvoiceRegister>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Реєстру з ID {id}", ex);
        }
    }

    public async Task<InvoiceRegister> UpdateRegisterAsync(InvoiceRegister invoiceRegister, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.UpdateAsync(invoiceRegister, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Реєстру з ID  {invoiceRegister.Id}", ex);
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
            throw new Exception($"Помилка при видаленні Реєстру з ID {id}", ex);
        }
    }

    public IQueryable<InvoiceRegister> GetRegisters(int page, int size)
    {
        try
        {
            return _repository.GetAll<InvoiceRegister>()
                .Skip((page - 1) * size)
                .Take(size);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку Реєстрів", ex);
        }
    }

    public IEnumerable<InvoiceRegister> SearchRegisters(string registerNumber)
    {
        try
        {
            return _repository.GetAll<InvoiceRegister>()
                .Where(r => r.RegisterNumber.ToLower().Contains(registerNumber.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Реєстру за номером {registerNumber}", ex);
        }
    }
}