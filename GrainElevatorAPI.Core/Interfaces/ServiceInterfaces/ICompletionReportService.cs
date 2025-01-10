using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface ICompletionReportService
{
    Task<CompletionReport> CreateCompletionReportAsync(string reportNumber,
        List<int> registerIds,
        List<int> operationIds,
        int createdById,
        CancellationToken cancellationToken);

    Task<CompletionReport> CalculateReportCostAsync(
        int reportId, 
        int priceListId,
        int modifiedById,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<CompletionReport>> GetCompletionReports(int page, int size, CancellationToken cancellationToken);
    Task<CompletionReport> GetCompletionReportByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<(IEnumerable<CompletionReport>, int)> SearchCompletionReportsAsync(
        string? reportNumber,
        DateTime? reportDate,
        double? physicalWeightReport,
        decimal? totalCost,
        string? supplierTitle,
        string? productTitle,
        string? createdByName,
        int page,
        int size,
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken);
    
    Task<CompletionReport> UpdateCompletionReportAsync(
        int id, 
        string? reportNumber,
        DateTime? reportDate,
        int modifiedById, 
        CancellationToken cancellationToken);
    Task<CompletionReport> SoftDeleteCompletionReportAsync(CompletionReport completionReport, int removedById, CancellationToken cancellationToken);
    Task<CompletionReport> RestoreRemovedCompletionReportAsync(CompletionReport completionReport, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteCompletionReportAsync(int id, CancellationToken cancellationToken);
}