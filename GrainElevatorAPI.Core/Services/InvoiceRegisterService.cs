using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Calculators.Impl;

namespace GrainElevatorAPI.Core.Services;

public class InvoiceRegisterService : IInvoiceRegisterService
{
    private readonly IRepository _repository;
    private readonly IRegisterCalculator _calculator;

    public InvoiceRegisterService(IRepository repository, IRegisterCalculator calculator)
    {
        _repository = repository;
        _calculator = calculator;
    }

    public async Task<InvoiceRegister> CreateRegisterAsync(
        int supplierId, 
        int productId, 
        DateTime arrivalDate, 
        double weedImpurityBase, 
        double moistureBase, 
        IEnumerable<int> laboratoryCardIds, 
        int createdById)
    {
        try
        {
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
            
            return  await _repository.AddAsync(register);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при створенні Реєстру", ex);
        }
        
    }
    
    // допоміжний метод для генерації номера реєстрації
    private string GenerateRegisterNumber()
    {
        return $"№{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    public async Task<InvoiceRegister> GetRegisterByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync<InvoiceRegister>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Реєстру з ID {id}", ex);
        }
    }

    public async Task<InvoiceRegister> UpdateRegisterAsync(InvoiceRegister invoiceRegister, int modifiedById)
    {
        try
        {
            invoiceRegister.ModifiedAt = DateTime.UtcNow;
            invoiceRegister.ModifiedById = modifiedById;
            
            await _repository.UpdateAsync(invoiceRegister);
            await _repository.SaveChangesAsync();
            
            return invoiceRegister;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Реєстру з ID  {invoiceRegister.Id}", ex);
        }
    }

    public async Task<bool> DeleteRegisterAsync(int id)
    {
        try
        {
            var register = await _repository.GetByIdAsync<InvoiceRegister>(id);
            if (register != null)
            {
                await _repository.DeleteAsync<InvoiceRegister>(id);
                await _repository.SaveChangesAsync();
                
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Реєстру з ID {id}", ex);
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
            throw new Exception("Помилка сервісу при отриманні списку Реєстрів", ex);
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
            throw new Exception($"Помилка сервісу при отриманні Реєстру за номером {registerNumber}", ex);
        }
    }
}