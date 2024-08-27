using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IProductTitleService
{
    Task<ProductTitle> AddProductTitleAsync(string  title);
    Task<ProductTitle> GetProductTitleByIdAsync(int id);
    Task<ProductTitle> UpdateProductTitleAsync(ProductTitle productTitle);
    Task<bool> DeleteProductTitleAsync(int id);
    IEnumerable<ProductTitle> GetProductTitles(int page, int size);
    IEnumerable<ProductTitle> SearchProductTitle(string title);
}