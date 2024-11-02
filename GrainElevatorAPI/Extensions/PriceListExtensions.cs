using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;

namespace GrainElevatorAPI.Extensions;

public static class PriceListExtensions
{
    public static void UpdateFromRequest(this PriceList priceList, PriceListUpdateRequest request)
    {
        priceList.ProductId = request.ProductId ?? priceList.ProductId;
    }
}