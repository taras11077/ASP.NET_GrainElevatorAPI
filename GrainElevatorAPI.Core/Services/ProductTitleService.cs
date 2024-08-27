using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class ProductTitleService : IProductTitleService
{
    private readonly IRepository _repository;

    public ProductTitleService(IRepository repository)
    {
        _repository = repository;
    }
    
    public Task<ProductTitle> AddProductTitle(string title)
    {
        throw new NotImplementedException();
    }

    public Task<ProductTitle> GetProductTitleById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductTitle> UpdateProductTitle(ProductTitle productTitle)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProductTitle(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ProductTitle> GetProductTitle(int page, int size)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ProductTitle> SearchProductTitle(string title)
    {
        throw new NotImplementedException();
    }
}