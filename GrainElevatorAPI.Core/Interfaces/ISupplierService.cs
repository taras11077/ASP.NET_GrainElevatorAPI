using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface ISupplierService
{
    Task<Supplier> AddSupplierAsync(string  title);
    Task<Supplier> GetSupplierByIdAsync(int id);
    Task<Supplier> UpdateSupplierAsync(Supplier supplier);
    Task<bool> DeleteSupplierAsync(int id);
    IEnumerable<Supplier> GetSuppliers(int page, int size);
    IEnumerable<Supplier> SearchSupplier(string title);
    
}
