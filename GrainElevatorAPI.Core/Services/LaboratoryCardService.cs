using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class LaboratoryCardService : ILaboratoryCardService
{
    private readonly IRepository _repository;

    public LaboratoryCardService(IRepository repository) => _repository = repository;


    public async Task<LaboratoryCard> CreateLaboratoryCardAsync(LaboratoryCard laboratoryCard, int createdById)
    {
        try
        {
            laboratoryCard.CreatedAt = DateTime.UtcNow;
            laboratoryCard.CreatedById = createdById;
            
            await _repository.AddAsync(laboratoryCard);
            await _repository.SaveChangesAsync();
            
            return laboratoryCard;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервіса при додаванні Лабораторної карточки", ex);
        }
    }
    public IQueryable<LaboratoryCard> GetLaboratoryCards(int page, int size)
    {
        try
        {
            return _repository.GetAll<LaboratoryCard>()
                .Skip((page - 1) * size)
                .Take(size);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервіса при отриманні списку Лабораторних карточок", ex);
        }
    }
    public async Task<LaboratoryCard> GetLaboratoryCardByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync<LaboratoryCard>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при отриманні Лабораторної карточки з ID {id}", ex);
        }
    }

     public IEnumerable<LaboratoryCard> SearchLaboratoryCards(int? id,
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
        int size)
    {
        try
        {
            var query = GetLaboratoryCards(page, size)
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
            
            return query.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервіса при пошуку Лабораторних карточок", ex);
        }
    }
    
    public async Task<LaboratoryCard> UpdateLaboratoryCardAsync(LaboratoryCard laboratoryCard, int modifiedById)
    {
        try
        {
            laboratoryCard.ModifiedAt = DateTime.UtcNow;
            laboratoryCard.ModifiedById = modifiedById;
            
            await _repository.UpdateAsync(laboratoryCard);
            await _repository.SaveChangesAsync();
            
            return laboratoryCard;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при оновленні Лабораторної карточки з ID  {laboratoryCard.Id}", ex);
        }
    }

    public async Task<LaboratoryCard> SoftDeleteLaboratoryCardAsync(LaboratoryCard laboratoryCard, int removedById)
    {
        try
        {
            laboratoryCard.RemovedAt = DateTime.UtcNow;
            laboratoryCard.RemovedById = removedById;
            
            await _repository.UpdateAsync(laboratoryCard);
            await _repository.SaveChangesAsync();
            
            return laboratoryCard;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при видаленні Лабораторної карточки з ID  {laboratoryCard.Id}", ex);
        }
    }
    
    public async Task<LaboratoryCard> RestoreRemovedLaboratoryCardAsync(LaboratoryCard laboratoryCard, int restoredById)
    {
        try
        {
            laboratoryCard.RemovedAt = null;
            laboratoryCard.RestoredAt = DateTime.UtcNow;
            laboratoryCard.RestoreById = restoredById;
            
            await _repository.UpdateAsync(laboratoryCard);
            await _repository.SaveChangesAsync();
            
            return laboratoryCard;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при відновленні Лабораторної карточки з ID  {laboratoryCard.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteLaboratoryCardAsync(int id)
    {
        try
        {
            var laboratoryCard = await _repository.GetByIdAsync<LaboratoryCard>(id);
            if (laboratoryCard != null)
            {
                await _repository.DeleteAsync<LaboratoryCard>(id);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервіса при видаленні Лабораторної карточки з ID {id}", ex);
        }
    }
}