using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IProductService
{
    Task<Product> CreateProductAsync(Product product, int createdById);
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> UpdateProductAsync(Product product, int modifiedById);
    
    Task<Product> SoftDeleteProductAsync(Product product, int removedById);
    Task<Product> RestoreRemovedProductAsync(Product product, int restoredById);
    
    Task<bool> DeleteProductAsync(int id);
    IEnumerable<Product> GetProducts(int page, int size);
    IEnumerable<Product> SearchProduct(string title);
}