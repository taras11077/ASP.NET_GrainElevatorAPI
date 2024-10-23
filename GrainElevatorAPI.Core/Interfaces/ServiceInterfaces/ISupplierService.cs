using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface ISupplierService
{
    Task<Supplier> AddSupplierAsync(Supplier supplier, int createdById);
    IEnumerable<Supplier> GetSuppliers(int page, int size);
    Task<Supplier> GetSupplierByIdAsync(int id);
    IEnumerable<Supplier> SearchSupplier(string title);
    Task<Supplier> UpdateSupplierAsync(Supplier supplier, int modifiedById);
    Task<Supplier> SoftDeleteSupplierAsync(Supplier supplier, int removedById);
    Task<Supplier> RestoreRemovedSupplierAsync(Supplier supplier, int restoredById);
    Task<bool> DeleteSupplierAsync(int id);
}
