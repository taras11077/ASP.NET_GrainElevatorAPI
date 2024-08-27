using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class SupplierService : ISupplierService
{
    public Task<Supplier> AddSupplier(string title)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier> GetSupplierById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier> UpdateSupplier(Supplier supplier)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteSupplier(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Supplier> GetSupplier(int page, int size)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Supplier> SearchSupplier(string title)
    {
        throw new NotImplementedException();
    }
}