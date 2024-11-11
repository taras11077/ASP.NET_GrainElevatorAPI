using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using IPriceListService = GrainElevatorAPI.Core.Interfaces.ServiceInterfaces.IPriceListService;

namespace GrainElevatorAPI.Core.Services;

public class PriceListService: IPriceListService
{
    private readonly IRepository _repository;

    public PriceListService(IRepository repository) => _repository = repository;


    public async Task<PriceList> CreatePriceListAsync(int productId, IEnumerable<PriceListItem> items, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            var priceList = new PriceList()
            {
                ProductId = productId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = createdById,
                PriceListItems = items.ToList()
            };
            
            
            return await _repository.AddAsync(priceList, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при створенні Прайс-листа", ex);
        }
    }
    public async Task<IEnumerable<PriceList>> GetPriceLists(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<PriceList>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Прайс-листів", ex);
        }
    }
    public async Task<PriceList> GetPriceListByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<PriceList>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Прайс-листа з ID {id}", ex);
        }
    }

     public async Task<IEnumerable<PriceList>> SearchPriceLists(int? id,
         int? productId,
         int? createdById,
         int page,
         int size,
         CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<PriceList>()
                .Skip((page - 1) * size)
                .Take(size);

            if (id.HasValue)
            {
                query = query.Where(pl => pl.Id == id);
            }
            
            if (productId.HasValue)
                query = query.Where(pl => pl.ProductId == productId.Value);
            
            if (createdById.HasValue)
                query = query.Where(pl => pl.CreatedById == createdById.Value);
            
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Прайс-листів за параметрами", ex);
        }
    }
    
    public async Task<PriceList> UpdatePriceListAsync(int id, int? productId, List<int>? priceListItemIds, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            var priceListDb = await GetPriceListByIdAsync(id, cancellationToken);
            if (priceListDb == null)
            {
                throw new Exception($"Прайс-листа з ID {id} не знайдено.");
            }
            
            priceListDb.ProductId = productId ?? priceListDb.ProductId;

            if (priceListItemIds != null && priceListItemIds.Any())
            {
                priceListDb.PriceListItems = new List<PriceListItem>();
                
                foreach (var priceListItemId in priceListItemIds)
                {
                    priceListDb.PriceListItems.Add( await _repository.GetByIdAsync<PriceListItem>(priceListItemId, cancellationToken));
                }
            }
            
            priceListDb.ModifiedAt = DateTime.UtcNow;
            priceListDb.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(priceListDb, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Прайс-листа з ID  {id}", ex);
        }
    }

    public async Task<PriceList> SoftDeletePriceListAsync(PriceList priceList, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            priceList.RemovedAt = DateTime.UtcNow;
            priceList.RemovedById = removedById;
            
            return await _repository.UpdateAsync(priceList, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Прайс-листа з ID  {priceList.Id}", ex);
        }
    }
    
    public async Task<PriceList> RestoreRemovedPriceListAsync(PriceList priceList, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            priceList.RemovedAt = null;
            priceList.RestoredAt = DateTime.UtcNow;
            priceList.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(priceList, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Прайс-листа з ID  {priceList.Id}", ex);
        }
    }
    
    public async Task<bool> DeletePriceListAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var priceList = await _repository.GetByIdAsync<PriceList>(id, cancellationToken);
            if (priceList != null)
            {
                await _repository.DeleteAsync<PriceList>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Прайс-листа з ID {id}", ex);
        }
    }
}