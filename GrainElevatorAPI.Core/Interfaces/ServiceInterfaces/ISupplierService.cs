using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface ISupplierService
{
    Task<Supplier> CreateSupplierAsync(Supplier supplier, int createdById, CancellationToken cancellationToken);
    Task<IEnumerable<Supplier>> GetSuppliers(int page, int size, CancellationToken cancellationToken);
    Task<Supplier> GetSupplierByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Supplier>> SearchSupplier(string title, CancellationToken cancellationToken);
    Task<Supplier> UpdateSupplierAsync(Supplier supplier, int modifiedById, CancellationToken cancellationToken);
    Task<Supplier> SoftDeleteSupplierAsync(Supplier supplier, int removedById, CancellationToken cancellationToken);
    Task<Supplier> RestoreRemovedSupplierAsync(Supplier supplier, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteSupplierAsync(int id, CancellationToken cancellationToken);
}
