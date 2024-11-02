using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IPriceListItemService
{
    Task<PriceListItem> CreatePriceListItemAsync(PriceListItem priceListItem, int createdById, CancellationToken cancellationToken);
    Task<IEnumerable<PriceListItem>> GetPriceListItems(int page, int size, CancellationToken cancellationToken);
    Task<PriceListItem> GetPriceListItemByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IEnumerable<PriceListItem>> SearchPriceListItems(int? id,
        string? operationName,
        decimal? operationPrice,
        int? priceListId,
        int? createdById,
        int page,
        int size,
        CancellationToken cancellationToken);
    
    Task<PriceListItem> UpdatePriceListItemAsync(PriceListItem priceListItem, int modifiedById, CancellationToken cancellationToken);
    Task<PriceListItem> SoftDeletePriceListItemAsync(PriceListItem priceListItem, int removedById, CancellationToken cancellationToken);
    Task<PriceListItem> RestoreRemovedPriceListItemAsync(PriceListItem priceListItem, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeletePriceListItemAsync(int id, CancellationToken cancellationToken);

}