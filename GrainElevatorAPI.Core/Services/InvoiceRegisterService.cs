using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GrainElevatorAPI.Core.Services;

public class InvoiceRegisterService : IInvoiceRegisterService
{
    private readonly IRepository _repository;
    private readonly IWarehouseUnitService _warehouseUnitService;
    private readonly IRegisterCalculator _calculator;
    private readonly ILogger<InvoiceRegisterService> _logger;
    
    public InvoiceRegisterService(IRepository repository, IRegisterCalculator calculator, IWarehouseUnitService warehouseUnitService, ILogger<InvoiceRegisterService> logger)
    {
        _repository = repository;
        _calculator = calculator;
        _warehouseUnitService = warehouseUnitService;
        _logger = logger;
    }

    public async Task<InvoiceRegister> CreateInvoiceRegisterAsync(
        string registerNumber,
        DateTime arrivalDate,
        string supplierTitle, 
        string productTitle, 
        double weedImpurityBase, 
        double moistureBase, 
        int createdById, 
        CancellationToken cancellationToken)
    {
        try
        {
            // початок транзакції
            await _repository.BeginTransactionAsync(cancellationToken);
            
            // створення Реєстру (доробка продукції)
            var newRegister = await CreateRegisterAsync(
                registerNumber,
                arrivalDate,
                supplierTitle, 
                productTitle, 
                weedImpurityBase, 
                moistureBase, 
                createdById,
                cancellationToken);

            await _repository.AddAsync(newRegister, cancellationToken);
            
            // створення або оновлення складського юніта (переміщення продукції Реєстру на Склад)
            await _warehouseUnitService.WarehouseTransferAsync(newRegister, createdById, cancellationToken);
            
            // фіксація транзакції
            await _repository.CommitTransactionAsync(cancellationToken);
            
            return newRegister;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Бізнес-помилка: {ex.Message}");
            // відкат транзакції в разі помилки
            await _repository.RollbackTransactionAsync(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Помилка створення Реєстру: {ex.Message}");
            // відкат транзакції в разі помилки
            await _repository.RollbackTransactionAsync(cancellationToken);
            throw new Exception("Помилка сервісу під час створення Реєстру", ex);
        }
        
    }

    private async Task<InvoiceRegister> CreateRegisterAsync(
        string registerNumber,
        DateTime arrivalDate,
        string supplierTitle, 
        string productTitle, 
        double weedImpurityBase,
        double moistureBase,
        int createdById,
        CancellationToken cancellationToken)
    
    {
        // Отримання SupplierId
        var supplier = _repository.GetAll<Supplier>().FirstOrDefault(s => s.Title == supplierTitle);
        if (supplier == null)
            throw new InvalidOperationException($"Постачальника з назвою '{supplierTitle}' не знайдено.");
        
        var supplierId = supplier.Id;

        // Отримання ProductId
        var product = _repository.GetAll<Product>().FirstOrDefault(p => p.Title == productTitle);
        if (product == null)
            throw new InvalidOperationException($"Товар з назвою '{productTitle}' не знайдено.");
        
        var productId = product.Id;

        // Запит до LaboratoryCard
        var query = _repository.GetAll<LaboratoryCard>()
            .Include(lc => lc.InputInvoice)
            .ThenInclude(ii => ii.Product)
            .Include(lc => lc.InputInvoice.Supplier)
            .Where(lc => lc.InputInvoice.ArrivalDate.Date == arrivalDate.Date)
            .Where(lc => lc.RemovedAt == null)
            .Where(lc => lc.IsProduction == true);
            //.Where(lc => lc.IsFinalized == !true);
        
        if (!string.IsNullOrEmpty(supplierTitle))
            query = query.Where(lc => lc.InputInvoice.Supplier.Title == supplierTitle);
        
        if (!string.IsNullOrEmpty(productTitle))
            query = query.Where(lc => lc.InputInvoice.Product.Title == productTitle);

        var laboratoryCards = await query.ToListAsync(cancellationToken);

        if (laboratoryCards.Count == 0)
        {
            throw new InvalidOperationException($"Не знайдено жодної Лабораторної картки, яка б відповідала заданим Даті, Постачальнику і Товару..");
        }
        
        // Встановлення IsFinalized = true для кожної лабораторної карточки
        foreach (var card in laboratoryCards)
        {
            card.IsFinalized = true;
            await _repository.UpdateAsync(card, cancellationToken);
        }
        
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

        var calculatedRegister = MapLabCardsToProductionBatches(laboratoryCards, register);
        
        return calculatedRegister;
    }

    private InvoiceRegister MapLabCardsToProductionBatches(List<LaboratoryCard> laboratoryCards, InvoiceRegister register)
    {
        if(laboratoryCards == null || laboratoryCards.Count == 0)
            return register;
        
        
        foreach (var labCard in laboratoryCards)
        {
            var productionBatch = new ProductionBatch
            {
                LaboratoryCardId = labCard.Id,
            };
            register = (InvoiceRegister)_calculator.CalcProductionBatch(labCard, register, productionBatch);
        }
        
        return register;
    }
    
    
    public async Task<InvoiceRegister> UpdateInvoiceRegisterAsync(
        int id, 
        string? registerNumber, 
        double? weedImpurityBase, 
        double? moistureBase, 
        int modifiedById, 
        CancellationToken cancellationToken)
    {
        try
        {
            var invoiceRegisterDb = await _repository.GetByIdAsync<InvoiceRegister>(id, cancellationToken)
                                    ?? throw new InvalidOperationException($"InvoiceRegister with ID {id} not found.");
            
            // Отримання ID лабораторних карточок
            var laboratoryCardIds = invoiceRegisterDb.ProductionBatches?
                .Select(pb => pb.LaboratoryCardId)
                .Distinct()
                .ToList();

            if (laboratoryCardIds == null || !laboratoryCardIds.Any())
            {
                throw new InvalidOperationException($"No LaboratoryCards found for Register ID {invoiceRegisterDb.Id}.");
            }

            // Завантаження лабораторних карточок
            var laboratoryCards = await _repository.GetAll<LaboratoryCard>()
                .Where(lc => laboratoryCardIds.Contains(lc.Id))
                .ToListAsync(cancellationToken);

            if (!laboratoryCards.Any())
            {
                throw new InvalidOperationException($"No LaboratoryCards found for Register ID {invoiceRegisterDb.Id}.");
            }
            
            
            // початок транзакції
            await _repository.BeginTransactionAsync(cancellationToken);
            
            // видалення даних реєстру зі складського юніта
            await _warehouseUnitService.DeletingRegisterDataFromWarehouseUnitAsync(invoiceRegisterDb, modifiedById, cancellationToken);
            
            // видалення виробничих партій Реєстру
            foreach (var productionBatch in invoiceRegisterDb.ProductionBatches.ToList())
            {
                await _repository.DeleteAsync<ProductionBatch>(productionBatch.Id, cancellationToken);
            }
            
            // Оновлення даних Реєстру
            if (registerNumber != null || weedImpurityBase != null || moistureBase != null)
            {
                invoiceRegisterDb.RegisterNumber = registerNumber ?? invoiceRegisterDb.RegisterNumber;
                invoiceRegisterDb.WeedImpurityBase = weedImpurityBase ?? invoiceRegisterDb.WeedImpurityBase;
                invoiceRegisterDb.MoistureBase = moistureBase ?? invoiceRegisterDb.MoistureBase;
                invoiceRegisterDb.ModifiedById = modifiedById;
                
                // Скидання обчислюваних полів
                invoiceRegisterDb.PhysicalWeightReg = 0;
                invoiceRegisterDb.ShrinkageReg = 0;
                invoiceRegisterDb.WasteReg = 0;
                invoiceRegisterDb.AccWeightReg = 0;
                invoiceRegisterDb.QuantitiesDryingReg = 0; 
                
                // Очищення та повторне створення партій
                invoiceRegisterDb.ProductionBatches = new List<ProductionBatch>();
                invoiceRegisterDb = MapLabCardsToProductionBatches(laboratoryCards, invoiceRegisterDb);
            }
            
            // оновлення складського юніта (переміщення продукції оновленого Реєстру на Склад)
            await _warehouseUnitService.WarehouseTransferAsync(invoiceRegisterDb, modifiedById, cancellationToken);
            
            // Оновлення реєстру в БД
            await _repository.UpdateAsync(invoiceRegisterDb, cancellationToken);
            
            // фіксація транзакції
            await _repository.CommitTransactionAsync(cancellationToken);

            return invoiceRegisterDb;
        }
        catch (Exception ex)
        {
            // відкат транзакції в разі помилки
            await _repository.RollbackTransactionAsync(cancellationToken);
            throw new Exception($"Помилка сервісу під час оновлення Реєстру з ID  {id}", ex);
        }
    }

    public async Task<InvoiceRegister> GetInvoiceRegisterByIdAsync(int id, CancellationToken cancellationToken)
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


    public async Task<IEnumerable<InvoiceRegister>> GetInvoiceRegistersAsync(int page, int size, CancellationToken cancellationToken)
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

    
    public async Task<(IEnumerable<InvoiceRegister>, int)> SearchInvoiceRegistersAsync(
        string? registerNumber = null,
        DateTime? arrivalDate = null,
        int? physicalWeightReg = null,
        int? shrinkageReg = null,
        int? wasteReg = null,
        int? accWeightReg = null,
        double? weedImpurityBase = null,
        double? moistureBase = null,
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
            var query = _repository.GetAll<InvoiceRegister>()
                .Include(r => r.ProductionBatches)
                    .ThenInclude(pb => pb.LaboratoryCard)
                    .ThenInclude( lc => lc.InputInvoice)
                .Include(r => r.Product)
                .Include(r => r.Supplier)
                .Include(r => r.CreatedBy)
                .Where(r => r.RemovedAt == null);

            // Виклик методу фільтрації
            query = ApplyFilters(query, registerNumber, arrivalDate, physicalWeightReg, 
                shrinkageReg, wasteReg, accWeightReg, weedImpurityBase, 
                moistureBase, supplierTitle, productTitle, createdByName);

            // Виклик методу сортування
            query = ApplySorting(query, sortField, sortOrder);

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredRegisters = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredRegisters, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Реєстру за параметрами", ex);
        }
    }

    
    
    private IQueryable<InvoiceRegister> ApplyFilters(
        IQueryable<InvoiceRegister> query,
        string? registerNumber,
        DateTime? arrivalDate,
        int? physicalWeightReg,
        int? shrinkageReg,
        int? wasteReg,
        int? accWeightReg,
        double? weedImpurityBase,
        double? moistureBase,
        string? supplierTitle,
        string? productTitle,
        string? createdByName)
    {
        if (!string.IsNullOrEmpty(registerNumber))
        {
            query = query.Where(r => r.RegisterNumber == registerNumber);
        }

        if (arrivalDate.HasValue)
        {
            query = query.Where(r => r.ArrivalDate.Date == arrivalDate.Value.Date);
        }

        if (!string.IsNullOrEmpty(supplierTitle))
        {
            query = query.Where(r => r.Supplier.Title == supplierTitle);
        }

        if (!string.IsNullOrEmpty(productTitle))
        {
            query = query.Where(r => r.Product.Title == productTitle);
        }

        if (weedImpurityBase.HasValue)
        {
            query = query.Where(r => r.WeedImpurityBase == weedImpurityBase.Value);
        }

        if (moistureBase.HasValue)
        {
            query = query.Where(r => r.MoistureBase == moistureBase.Value);
        }

        if (physicalWeightReg.HasValue)
        {
            query = query.Where(r => r.PhysicalWeightReg == physicalWeightReg.Value);
        }

        if (accWeightReg.HasValue)
        {
            query = query.Where(r => r.AccWeightReg == accWeightReg.Value);
        }

        if (shrinkageReg.HasValue)
        {
            query = query.Where(r => r.ShrinkageReg == shrinkageReg.Value);
        }

        if (wasteReg.HasValue)
        {
            query = query.Where(r => r.WasteReg == wasteReg.Value);
        }

        if (!string.IsNullOrEmpty(createdByName))
        {
            query = query.Where(r => r.CreatedBy.LastName == createdByName);
        }
        
        return query;
    }

    
    private IQueryable<InvoiceRegister> ApplySorting(
        IQueryable<InvoiceRegister> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "registerNumber" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.RegisterNumber)
                : query.OrderByDescending(reg => reg.RegisterNumber),
            "arrivalDate" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.ArrivalDate)
                : query.OrderByDescending(reg => reg.ArrivalDate),
            "productTitle" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.Product.Title)
                : query.OrderByDescending(reg => reg.Product.Title),
            "supplierTitle" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.Supplier.Title)
                : query.OrderByDescending(reg => reg.Supplier.Title),
            "physicalWeightReg" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.PhysicalWeightReg)
                : query.OrderByDescending(reg => reg.PhysicalWeightReg),
            "shrinkageReg" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.ShrinkageReg)
                : query.OrderByDescending(reg => reg.ShrinkageReg),
            "wasteReg" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.WasteReg)
                : query.OrderByDescending(reg => reg.WasteReg),
            "accWeightReg" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.AccWeightReg)
                : query.OrderByDescending(reg => reg.AccWeightReg),
            "createdByName" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.CreatedBy.LastName)
                : query.OrderByDescending(reg => reg.CreatedBy.LastName),
            _ => query // Якщо поле не визначене
        };
    }

     
    public async Task<InvoiceRegister> SoftDeleteInvoiceRegisterAsync(InvoiceRegister invoiceRegister, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            // Позначення реєстру як видаленого
            invoiceRegister.RemovedAt = DateTime.UtcNow;
            invoiceRegister.RemovedById = removedById;

            // Отримання ID лабораторних карточок
            var laboratoryCardIds = invoiceRegister.ProductionBatches?
                .Select(pb => pb.LaboratoryCardId)
                .Distinct()
                .ToList();

            if (laboratoryCardIds == null || !laboratoryCardIds.Any())
            {
                throw new InvalidOperationException($"No LaboratoryCards found for Register ID {invoiceRegister.Id}.");
            }

            // Завантаження лабораторних карточок
            var laboratoryCards = await _repository.GetAll<LaboratoryCard>()
                .Where(lc => laboratoryCardIds.Contains(lc.Id))
                .ToListAsync(cancellationToken);

            if (!laboratoryCards.Any())
            {
                throw new InvalidOperationException($"No LaboratoryCards found for Register ID {invoiceRegister.Id}.");
            }

            // Оновлення властивості IsFinalized
            foreach (var card in laboratoryCards)
            {
                card.IsFinalized = false;
            }
            
            // Збереження змін у лабораторних карточках
            await _repository.SaveChangesAsync(cancellationToken);
            
            // видалення виробничих партій Реєстру
            foreach (var productionBatch in invoiceRegister.ProductionBatches.ToList())
            {
                await _repository.DeleteAsync<ProductionBatch>(productionBatch.Id, cancellationToken);
            }
            
            // видалення даних реєстру зі складського юніта
            await _warehouseUnitService.DeletingRegisterDataFromWarehouseUnitAsync(invoiceRegister, removedById, cancellationToken);

            // Збереження змін у реєстрі
            return await _repository.UpdateAsync(invoiceRegister, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу під час видалення Реєстру з ID  {invoiceRegister.Id}", ex);
        }
    }
    
    public async Task<InvoiceRegister> RestoreRemovedInvoiceRegisterAsync(InvoiceRegister invoiceRegister, int restoredById, CancellationToken cancellationToken)
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
    
    public async Task<bool> DeleteInvoiceRegisterAsync(int id, CancellationToken cancellationToken)
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