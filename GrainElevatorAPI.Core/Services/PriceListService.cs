using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using IPriceListService = GrainElevatorAPI.Core.Interfaces.ServiceInterfaces.IPriceListService;

namespace GrainElevatorAPI.Core.Services;

public class PriceListService: IPriceListService
{
    private readonly IRepository _repository;

    public PriceListService(IRepository repository) => _repository = repository;


    public async Task<PriceList> CreatePriceListAsync(string productTitle, IEnumerable<PriceListItem> items, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _repository.GetAll<Product>()
                .FirstOrDefaultAsync(p => p.Title == productTitle, cancellationToken);
        
            if (product == null)
                throw new KeyNotFoundException($"Продукта: {productTitle} не знайдено.");
            
            var priceList = new PriceList()
            {
                ProductId = product.Id,
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

     public async Task<(IEnumerable<PriceList>, int)> SearchPriceLists(
         string? productTitle,
         string? createdByName,
         int page,
         int size,
         string? sortField,
         string? sortOrder,
         CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<PriceList>()
                .Include(pl => pl.PriceListItems)
                    .ThenInclude(pli => pli.TechnologicalOperation)
                .Include(pl => pl.Product)
                .Include(cr => cr.CreatedBy)
                .Where(pl => pl.RemovedAt == null);

            // Виклик методу фільтрації
            query = ApplyFilters(query, productTitle, createdByName);

            // Виклик методу сортування
            query = ApplySorting(query, sortField, sortOrder);

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredPriceLists = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredPriceLists, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Прайс-листів за параметрами", ex);
        }
    }
     
    private IQueryable<PriceList> ApplyFilters(
        IQueryable<PriceList> query,
        string? productTitle,
        string? createdByName)
    {
        if (!string.IsNullOrEmpty(productTitle))
        {
            query = query.Where(pl => pl.Product.Title == productTitle);
        }
        if (!string.IsNullOrEmpty(createdByName))
        {
            query = query.Where(pl => pl.CreatedBy.LastName == createdByName);
        }
        
        return query;
    }
    
    private IQueryable<PriceList> ApplySorting(
        IQueryable<PriceList> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "title" => sortOrder == "asc"
                ? query.OrderBy(pl => pl.Product.Title)
                : query.OrderByDescending(pl => pl.Product.Title),
            "createdByName" => sortOrder == "asc"
                ? query.OrderBy(to => to.CreatedBy.LastName)
                : query.OrderByDescending(to => to.CreatedBy.LastName),
            _ => query // Якщо поле не визначене
        };
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