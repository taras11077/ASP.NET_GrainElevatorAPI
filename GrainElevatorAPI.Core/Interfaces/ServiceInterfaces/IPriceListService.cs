using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IPriceListService
{
    Task<PriceList> CreatePriceListAsync(int productId, IEnumerable<PriceListItem> items, int createdById, CancellationToken cancellationToken);
    Task<IEnumerable<PriceList>> GetPriceLists(int page, int size, CancellationToken cancellationToken);
    Task<PriceList> GetPriceListByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<(IEnumerable<PriceList>, int)> SearchPriceLists(
        string? productTitle,
        string? createdByName,
        int page,
        int size,
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken);
    
    Task<PriceList> UpdatePriceListAsync(int id, int? productId, List<int>? priceListItemIds, int modifiedById, CancellationToken cancellationToken);
    Task<PriceList> SoftDeletePriceListAsync(PriceList priceList, int removedById, CancellationToken cancellationToken);
    Task<PriceList> RestoreRemovedPriceListAsync(PriceList priceList, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeletePriceListAsync(int id, CancellationToken cancellationToken);
}