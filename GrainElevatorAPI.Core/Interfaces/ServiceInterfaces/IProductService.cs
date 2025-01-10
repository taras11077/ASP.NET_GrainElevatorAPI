using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IProductService
{
    Task<Product> CreateProductAsync(Product product, int createdById, CancellationToken cancellationToken);
    Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken);
    Task<Product> UpdateProductAsync(Product product, int modifiedById, CancellationToken cancellationToken);
    
    Task<Product> SoftDeleteProductAsync(Product product, int removedById, CancellationToken cancellationToken);
    Task<Product> RestoreRemovedProductAsync(Product product, int restoredById, CancellationToken cancellationToken);
    
    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProducts(int page, int size, CancellationToken cancellationToken);
    Task<(IEnumerable<Product>, int)> SearchProductsAsync(
        string? title,
        string? createdByName,
        int page,
        int size,
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken);
}