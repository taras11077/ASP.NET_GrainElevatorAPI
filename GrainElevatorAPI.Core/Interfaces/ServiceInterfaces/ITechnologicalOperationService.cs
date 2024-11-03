using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface ITechnologicalOperationService
{
    Task<TechnologicalOperation> CreateTechnologicalOperationAsync(TechnologicalOperation technologicalOperation, int createdById, CancellationToken cancellationToken);
    Task<IEnumerable<TechnologicalOperation>> GetTechnologicalOperations(int page, int size, CancellationToken cancellationToken);
    Task<TechnologicalOperation> GetTechnologicalOperationByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<TechnologicalOperation>> SearchTechnologicalOperation(string title, CancellationToken cancellationToken);
    Task<TechnologicalOperation> UpdateTechnologicalOperationAsync(TechnologicalOperation technologicalOperation, int modifiedById, CancellationToken cancellationToken);
    Task<TechnologicalOperation> SoftDeleteTechnologicalOperationAsync(TechnologicalOperation technologicalOperation, int removedById, CancellationToken cancellationToken);
    Task<TechnologicalOperation> RestoreRemovedTechnologicalOperationAsync(TechnologicalOperation technologicalOperation, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteTechnologicalOperationAsync(int id, CancellationToken cancellationToken);
}