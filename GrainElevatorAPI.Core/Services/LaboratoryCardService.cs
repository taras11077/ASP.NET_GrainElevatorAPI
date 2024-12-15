using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class LaboratoryCardService : ILaboratoryCardService
{
    private readonly IRepository _repository;

    public LaboratoryCardService(IRepository repository) => _repository = repository;
    
    public async Task<LaboratoryCard> CreateLaboratoryCardAsync(LaboratoryCard laboratoryCard, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            laboratoryCard.CreatedAt = DateTime.UtcNow;
            laboratoryCard.CreatedById = createdById;
            
            var invoice = await _repository.GetByIdAsync<InputInvoice>(laboratoryCard.InputInvoiceId, cancellationToken);
            if (invoice == null)
            {
                throw new InvalidOperationException($"Вхідна накладна з ID {laboratoryCard.InputInvoiceId} не знайдена.");
            }
            
            var createdLaboratoryCard = await _repository.AddAsync<LaboratoryCard>(laboratoryCard, cancellationToken);

            if (createdLaboratoryCard is LaboratoryCard)
            {
                invoice.IsFinalized = true;
                await _repository.UpdateAsync<InputInvoice>(invoice, cancellationToken);
            }

            return createdLaboratoryCard;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при створенні Лабораторної карточки", ex);
        }
    }
    public async Task<IEnumerable<LaboratoryCard>> GetLaboratoryCards(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<LaboratoryCard>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Лабораторних карточок", ex);
        }
    }
    public async Task<LaboratoryCard> GetLaboratoryCardByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<LaboratoryCard>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Лабораторної карточки з ID {id}", ex);
        }
    }

     public async Task<(IEnumerable<LaboratoryCard>, int)> SearchLaboratoryCards(
         int? id = null,
        string? labCardNumber = null,
        double? weedImpurity = null,
        double? moisture = null,
        bool? isProduction = null,
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
            var query = _repository.GetAll<LaboratoryCard>()
                .Include(ii => ii.CreatedBy)
                .Include(lc => lc.InputInvoice) 
                .ThenInclude(ii => ii.Product) 
                .Include(lc => lc.InputInvoice.Supplier) 
                .AsQueryable();

            // Фільтрація
            if (id.HasValue)
            {
                query = query.Where(lc => lc.Id == id);
            }
            
            if (!string.IsNullOrEmpty(labCardNumber))
            {
                query = query.Where(lc => lc.LabCardNumber == labCardNumber);
            }

            if (arrivalDate.HasValue)
            {
                query = query.Where(lc => lc.InputInvoice.ArrivalDate.Date == arrivalDate.Value.Date);
            }
            
            if (!string.IsNullOrEmpty(vehicleNumber))
                query = query.Where(lc => lc.InputInvoice.VehicleNumber == vehicleNumber);
            
            if (physicalWeight.HasValue)
                query = query.Where(lc => lc.InputInvoice.PhysicalWeight == physicalWeight.Value);
            
            if (!string.IsNullOrEmpty(supplierTitle))
                query = query.Where(lc => lc.InputInvoice.Supplier.Title == supplierTitle);

            if (!string.IsNullOrEmpty(productTitle))
                query = query.Where(lc => lc.InputInvoice.Product.Title == productTitle);
            

            if (weedImpurity.HasValue)
                query = query.Where(lc => lc.WeedImpurity == weedImpurity.Value);

            if (moisture.HasValue)
                query = query.Where(lc => lc.Moisture == moisture.Value);
            
            if (isProduction.HasValue)
                query = query.Where(lc => lc.IsProduction == isProduction.Value);
            
            if (!string.IsNullOrEmpty(createdByName))
                query = query.Where(lc => lc.CreatedBy.LastName == createdByName);

            if (removedAt.HasValue)
                query = query.Where(lc => lc.RemovedAt == removedAt.Value);
            
            
            // Сортування
            if (!string.IsNullOrEmpty(sortField))
            {
                query = sortField switch
                {
                    "labCardNumber" => sortOrder == "asc"
                        ? query.OrderBy(lc => lc.LabCardNumber)
                        : query.OrderByDescending(lc => lc.LabCardNumber),
                    "arrivalDate" => sortOrder == "asc"
                        ? query.OrderBy(lc => lc.InputInvoice.ArrivalDate)
                        : query.OrderByDescending(lc => lc.InputInvoice.ArrivalDate),
                    "physicalWeight" => sortOrder == "asc"
                        ? query.OrderBy(lc => lc.InputInvoice.PhysicalWeight)
                        : query.OrderByDescending(lc => lc.InputInvoice.PhysicalWeight),
                    "productTitle" => sortOrder == "asc"
                        ? query.OrderBy(lc => lc.InputInvoice.Product.Title)
                        : query.OrderByDescending(lc => lc.InputInvoice.Product.Title),
                    "supplierTitle" => sortOrder == "asc"
                        ? query.OrderBy(lc => lc.InputInvoice.Supplier.Title)
                        : query.OrderByDescending(lc => lc.InputInvoice.Supplier.Title),
                    "weedImpurity" => sortOrder == "asc"
                        ? query.OrderBy(lc => lc.WeedImpurity)
                        : query.OrderByDescending(lc => lc.WeedImpurity),
                    "moisture" => sortOrder == "asc"
                        ? query.OrderBy(lc => lc.Moisture)
                        : query.OrderByDescending(lc => lc.Moisture),
                    "createdByName" => sortOrder == "asc"
                        ? query.OrderBy(lc => lc.CreatedBy.LastName)
                        : query.OrderByDescending(lc => lc.CreatedBy.LastName),
                    _ => query // Без сортування, якщо поле не вказано
                };
            }
        

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredCards = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
            
            
            return (filteredCards, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Лабораторних карточок за параметрами", ex);
        }
    }
    
    public async Task<LaboratoryCard> UpdateLaboratoryCardAsync(LaboratoryCard laboratoryCard, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            laboratoryCard.ModifiedAt = DateTime.UtcNow;
            laboratoryCard.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(laboratoryCard, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Лабораторної карточки з ID  {laboratoryCard.Id}", ex);
        }
    }

    public async Task<LaboratoryCard> SoftDeleteLaboratoryCardAsync(LaboratoryCard laboratoryCard, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            laboratoryCard.RemovedAt = DateTime.UtcNow;
            laboratoryCard.RemovedById = removedById;
            
            var invoice = await _repository.GetByIdAsync<InputInvoice>(laboratoryCard.InputInvoiceId, cancellationToken);
            if (invoice == null)
            {
                throw new InvalidOperationException($"В лабораторній карті Вхідна накладна з ID {laboratoryCard.InputInvoiceId} не знайдена.");
            }
            
            var deletedLaboratoryCard = await _repository.UpdateAsync<LaboratoryCard>(laboratoryCard, cancellationToken);

            if (deletedLaboratoryCard is LaboratoryCard)
            {
                invoice.IsFinalized = false;
                await _repository.UpdateAsync<InputInvoice>(invoice, cancellationToken);
            }

            return deletedLaboratoryCard;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Лабораторної карточки з ID  {laboratoryCard.Id}", ex);
        }
    }
    
    public async Task<LaboratoryCard> RestoreRemovedLaboratoryCardAsync(LaboratoryCard laboratoryCard, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            laboratoryCard.RemovedAt = null;
            laboratoryCard.RestoredAt = DateTime.UtcNow;
            laboratoryCard.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(laboratoryCard, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Лабораторної карточки з ID  {laboratoryCard.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteLaboratoryCardAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var laboratoryCard = await _repository.GetByIdAsync<LaboratoryCard>(id, cancellationToken);
            if (laboratoryCard != null)
            {
                await _repository.DeleteAsync<LaboratoryCard>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Лабораторної карточки з ID {id}", ex);
        }
    }
}