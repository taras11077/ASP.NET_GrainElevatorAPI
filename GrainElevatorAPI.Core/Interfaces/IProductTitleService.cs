using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IProductTitleService
{
    Task<ProductTitle> AddProductTitle(string  title);
    Task<ProductTitle> GetProductTitleById(int id);
    Task<ProductTitle> UpdateProductTitle(ProductTitle productTitle);
    Task<bool> DeleteProductTitle(int id);
    IEnumerable<ProductTitle> GetProductTitle(int page, int size);
    IEnumerable<ProductTitle> SearchProductTitle(string title);
}