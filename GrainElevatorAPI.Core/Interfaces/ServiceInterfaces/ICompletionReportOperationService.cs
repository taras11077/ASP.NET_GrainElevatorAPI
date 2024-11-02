using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface ICompletionReportOperationService
{
    Task<CompletionReportOperation> CreateCompletionReportOperationAsync(CompletionReportOperation сompletionReportOperation, int createdById, CancellationToken cancellationToken);
    Task<IEnumerable<CompletionReportOperation>> GetCompletionReportOperations(int page, int size, CancellationToken cancellationToken);
    Task<CompletionReportOperation> GetCompletionReportOperationByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IEnumerable<CompletionReportOperation>> SearchCompletionReportOperations(int? id,
        int? technologicalOperationId,
        double? amount,
        int? completionReportId,
        int? createdById,
        int page,
        int size,
        CancellationToken cancellationToken);
    
    Task<CompletionReportOperation> UpdateCompletionReportOperationAsync(CompletionReportOperation сompletionReportOperation, int modifiedById, CancellationToken cancellationToken);
    Task<CompletionReportOperation> SoftDeleteCompletionReportOperationAsync(CompletionReportOperation сompletionReportOperation, int removedById, CancellationToken cancellationToken);
    Task<CompletionReportOperation> RestoreRemovedCompletionReportOperationAsync(CompletionReportOperation сompletionReportOperation, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteCompletionReportOperationAsync(int id, CancellationToken cancellationToken);
}