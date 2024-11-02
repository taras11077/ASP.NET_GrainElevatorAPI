using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;

namespace GrainElevatorAPI.Extensions;

public static class PriceListItemExtensions
{
    public static void UpdateFromRequest(this PriceListItem priceListItem, PriceListItemUpdateRequest request)
    {
        priceListItem.TechnologicalOperationId = request.TechnologicalOperationId ?? priceListItem.TechnologicalOperationId;
        priceListItem.OperationPrice = request.OperationPrice ?? priceListItem.OperationPrice;

    }
}