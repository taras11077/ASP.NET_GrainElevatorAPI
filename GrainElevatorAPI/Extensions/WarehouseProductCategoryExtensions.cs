using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.DTOs.Requests;

namespace GrainElevatorAPI.Extensions;

public static class WarehouseProductCategoryExtensions
{
    public static void UpdateFromRequest(this WarehouseProductCategory warehouseProductCategory, WarehouseProductCategoryUpdateRequest request)
    {
        warehouseProductCategory.Title = request.Title ?? warehouseProductCategory.Title;
        warehouseProductCategory.Value = request.Value ?? warehouseProductCategory.Value;

    }
}