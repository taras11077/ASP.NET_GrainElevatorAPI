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
        // початок транзакції
        await _repository.BeginTransactionAsync(cancellationToken);
        
        try
        {
            laboratoryCard.CreatedAt = DateTime.UtcNow;
            laboratoryCard.CreatedById = createdById;
            
            var invoice = await _repository.GetByIdAsync<InputInvoice>(laboratoryCard.InputInvoiceId, cancellationToken);
            if (invoice == null)
            {
                throw new InvalidOperationException($"Вхідна накладна з ID {laboratoryCard.InputInvoiceId} не знайдена.");
            }
            
            invoice.IsFinalized = true;
            await _repository.UpdateAsync<InputInvoice>(invoice, cancellationToken);
            
            var createdLaboratoryCard = await _repository.AddAsync<LaboratoryCard>(laboratoryCard, cancellationToken);
            
            // фіксація транзакції
            await _repository.CommitTransactionAsync(cancellationToken);

            return createdLaboratoryCard;
        }
        catch (Exception ex)
        {
            // відкат транзакції в разі помилки
            await _repository.RollbackTransactionAsync(cancellationToken);
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

     public async Task<IEnumerable<LaboratoryCard>> SearchLaboratoryCards(int? id,
        string? labCardNumber,
        double? weedImpurity,
        double? moisture,
        bool? isProduction,
        DateTime? arrivalDate,
        string? vehicleNumber,
        int? physicalWeight,
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
            var query = _repository.GetAll<LaboratoryCard>()
                .Include(lc => lc.InputInvoice)
                .AsQueryable();

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
            
            if (supplierId.HasValue)
                query = query.Where(lc => lc.InputInvoice.SupplierId == supplierId.Value);

            if (productId.HasValue)
                query = query.Where(lc => lc.InputInvoice.ProductId == productId.Value);
            

            if (weedImpurity.HasValue)
                query = query.Where(lc => lc.WeedImpurity == weedImpurity.Value);

            if (moisture.HasValue)
                query = query.Where(lc => lc.Moisture == moisture.Value);
            
            if (isProduction.HasValue)
                query = query.Where(lc => lc.IsProduction == isProduction.Value);
            
            if (createdById.HasValue)
                query = query.Where(lc => lc.CreatedById == createdById.Value);

            if (removedAt.HasValue)
                query = query.Where(lc => lc.RemovedAt == removedAt.Value);
            
            return await query.ToListAsync(cancellationToken);
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
            
            return await _repository.UpdateAsync(laboratoryCard, cancellationToken);
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