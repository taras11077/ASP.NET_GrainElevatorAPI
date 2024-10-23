using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IProductionBatchService
{
    Task<ProductionBatch> AddProductionBatchAsync(ProductionBatch productionBatch);
    Task<ProductionBatch> GetProductionBatchByIdAsync(int id);
    Task<ProductionBatch> UpdateProductionBatchAsync(ProductionBatch productionBatch);
    Task<bool> DeleteProductionBatchAsync(int id);
    IQueryable<ProductionBatch> GetProductionBatch(int page, int size);
}