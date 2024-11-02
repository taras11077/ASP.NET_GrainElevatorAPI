﻿using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface ICompletionReportService
{
    Task<CompletionReport> CreateCompletionReportAsync(string reportNumber,
        List<int> registers,
        int createdById,
        CancellationToken cancellationToken);
    Task<IEnumerable<CompletionReport>> GetCompletionReports(int page, int size, CancellationToken cancellationToken);
    Task<CompletionReport> GetCompletionReportByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IEnumerable<CompletionReport>> SearchCompletionReports(int? id,
        string? reportNumber,
        DateTime? reportDate,
        double? reportQuantitiesDrying,
        int? reportPhysicalWeight,
        int? supplierId,
        int? productId,
        int? createdById,
        int page,
        int size,
        CancellationToken cancellationToken);
    
    Task<CompletionReport> UpdateCompletionReportAsync(CompletionReport completionReport, int modifiedById, CancellationToken cancellationToken);
    Task<CompletionReport> SoftDeleteCompletionReportAsync(CompletionReport completionReport, int removedById, CancellationToken cancellationToken);
    Task<CompletionReport> RestoreRemovedCompletionReportAsync(CompletionReport completionReport, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteCompletionReportAsync(int id, CancellationToken cancellationToken);
}