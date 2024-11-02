using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using IPriceListItemService = GrainElevatorAPI.Core.Interfaces.ServiceInterfaces.IPriceListItemService;

namespace GrainElevatorAPI.Core.Services;

public class PriceListItemService: IPriceListItemService
{
    private readonly IRepository _repository;

    public PriceListItemService(IRepository repository) => _repository = repository;


    public async Task<PriceListItem> CreatePriceListItemAsync(PriceListItem priceListItem, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            priceListItem.CreatedAt = DateTime.UtcNow;
            priceListItem.CreatedById = createdById;
            
            return await _repository.AddAsync(priceListItem, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при створенні Елемента прайс-листа", ex);
        }
    }
    public async Task<IEnumerable<PriceListItem>> GetPriceListItems(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<PriceListItem>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Елементів прайс-листа", ex);
        }
    }
    public async Task<PriceListItem> GetPriceListItemByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<PriceListItem>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Елемента прайс-листа з ID {id}", ex);
        }
    }

     public async Task<IEnumerable<PriceListItem>> SearchPriceListItems(int? id, 
         int? technologicalOperationId,
        decimal? operationPrice,
        int? priceListId,
        int? createdById,
        int page,
        int size,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<PriceListItem>()
                .Skip((page - 1) * size)
                .Take(size);

            if (id.HasValue)
            {
                query = query.Where(pli => pli.Id == id);
            }
            
            if (technologicalOperationId.HasValue)
            {
                query = query.Where(pli => pli.TechnologicalOperationId == technologicalOperationId);
            }

            if (operationPrice.HasValue)
            {
                query = query.Where(pli => pli.OperationPrice == operationPrice.Value);
            }

            
            if (priceListId.HasValue)
                query = query.Where(pli => pli.PriceListId == priceListId.Value);
            
            if (createdById.HasValue)
                query = query.Where(pli => pli.CreatedById == createdById.Value);
            
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Елементів прайс-листа за параметрами", ex);
        }
    }
    
    public async Task<PriceListItem> UpdatePriceListItemAsync(PriceListItem priceListItem, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            priceListItem.ModifiedAt = DateTime.UtcNow;
            priceListItem.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(priceListItem, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Елемента прайс-листа з ID  {priceListItem.Id}", ex);
        }
    }

    public async Task<PriceListItem> SoftDeletePriceListItemAsync(PriceListItem priceListItem, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            priceListItem.RemovedAt = DateTime.UtcNow;
            priceListItem.RemovedById = removedById;
            
            return await _repository.UpdateAsync(priceListItem, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Елемента прайс-листа з ID  {priceListItem.Id}", ex);
        }
    }
    
    public async Task<PriceListItem> RestoreRemovedPriceListItemAsync(PriceListItem priceListItem, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            priceListItem.RemovedAt = null;
            priceListItem.RestoredAt = DateTime.UtcNow;
            priceListItem.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(priceListItem, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Елемента прайс-листа з ID  {priceListItem.Id}", ex);
        }
    }
    
    public async Task<bool> DeletePriceListItemAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var priceListItem = await _repository.GetByIdAsync<PriceListItem>(id, cancellationToken);
            if (priceListItem != null)
            {
                await _repository.DeleteAsync<PriceListItem>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Елемента прайс-листа з ID {id}", ex);
        }
    }
}