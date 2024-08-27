using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface ISupplierService
{
    Task<Supplier> AddSupplier(string  title);
    Task<Supplier> GetSupplierById(int id);
    Task<Supplier> UpdateSupplier(Supplier supplier);
    Task<bool> DeleteSupplier(int id);
    IEnumerable<Supplier> GetSupplier(int page, int size);
    IEnumerable<Supplier> SearchSupplier(string title);
    
}
