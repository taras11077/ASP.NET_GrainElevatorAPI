using GrainElevatorAPI.Core.Calculators.Impl;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using ICompletionReportService = GrainElevatorAPI.Core.Interfaces.ServiceInterfaces.ICompletionReportService;

namespace GrainElevatorAPI.Core.Services;

public class CompletionReportService: ICompletionReportService
{
    private readonly IRepository _repository;

    public CompletionReportService(IRepository repository) => _repository = repository;


    public async Task<CompletionReport> CreateCompletionReportAsync(
    string reportNumber,
    List<int> registerIds,
    List<int> operationIds,
    int createdById,
    CancellationToken cancellationToken)
{
    try
    {
        var registers = await _repository.GetAll<InvoiceRegister>()
            .Where(r => registerIds.Contains(r.Id))
            .ToListAsync(cancellationToken);
        
        var operations = await _repository.GetAll<TechnologicalOperation>()
            .Where(op => operationIds.Contains(op.Id))
            .ToListAsync(cancellationToken);
        
        var completionReport = new CompletionReport
        {
            ReportNumber = reportNumber,
            ReportDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedById = createdById
        };

        // використання калькулятора для вагових характеристик
        var сompletionReportCalculator = new CompletionReportCalculator();
        сompletionReportCalculator.CalculateWeights(registers, completionReport);

        // додавання операцій до звіту
        foreach (var operation in operations)
        {
            var amount = MapOperationToReportField(operation, completionReport) ?? 0;

            completionReport.CompletionReportOperations.Add(new CompletionReportOperation
            {
                TechnologicalOperationId = operation.Id,
                Amount = amount,
                CompletionReportId = completionReport.Id,
                OperationCost = 0
            });
        }

        return await _repository.AddAsync(completionReport, cancellationToken);
    }
    catch (Exception ex)
    {
        throw new Exception("Помилка сервісу при створенні Акта виконаних робіт", ex);
    }
}
    
    // створення відповідності технологічних операцій до полів звіту
    private int? MapOperationToReportField(TechnologicalOperation operation, CompletionReport report)
    {
        return operation.Title switch
        {
            "Приймання" => report.PhysicalWeightReport,
            "Первинне очищення" => report.PhysicalWeightReport,
            "Сушіння" => report.QuantitiesDryingReport,
            "Відвантаження" => report.AccWeightReport,
            "Утилізація відходів" => report.WasteReport,
            _ => null // якщо операція не знайдена, повертаємо null
        };
    }


    
    public async Task<CompletionReport> CalculateReportCostAsync(
        int reportId, 
        int priceListId,
        int modifiedById,
        CancellationToken cancellationToken)
    {
        
        
        var completionReport = await _repository.GetByIdAsync<CompletionReport>(reportId, cancellationToken);

        if (completionReport == null)
            throw new Exception($"Акт виконаних робіт з ID {reportId} не знайдено");

        var priceList = await _repository.GetByIdAsync<PriceList>(priceListId, cancellationToken);
        if (priceList == null)
            throw new Exception($"Прайс-лист з ID {priceListId} не знайдено");

        var сompletionReportCalculator = new CompletionReportCalculator();
        сompletionReportCalculator.CalculateTotalCost(completionReport, priceList);

        await _repository.SaveChangesAsync(cancellationToken);

        return completionReport;
    }

    
    
    public async Task<IEnumerable<CompletionReport>> GetCompletionReports(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<CompletionReport>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Актів виконаних робіт", ex);
        }
    }
    public async Task<CompletionReport> GetCompletionReportByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<CompletionReport>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Акта виконаних робіт з ID {id}", ex);
        }
    }

     public async Task<IEnumerable<CompletionReport>> SearchCompletionReports(int? id,
         string? reportNumber,
         DateTime? reportDate,
         int? quantitiesDryingReport,
         int? physicalWeightReport,
         int? supplierId,
         int? productId,
         int? createdById,
         int page,
         int size,
         CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<CompletionReport>()
                .Skip((page - 1) * size)
                .Take(size);

            if (id.HasValue)
            {
                query = query.Where(cr => cr.Id == id);
            }
            
            if (!string.IsNullOrEmpty(reportNumber))
            {
                query = query.Where(cr => cr.ReportNumber == reportNumber);
            }
            
            if (reportDate.HasValue)
            {
                query = query.Where(cr => cr.ReportDate.Date == reportDate.Value.Date);
            }
            
            if (quantitiesDryingReport.HasValue)
                query = query.Where(cr => cr.QuantitiesDryingReport == quantitiesDryingReport.Value);
            
            if (physicalWeightReport.HasValue)
                query = query.Where(cr => cr.PhysicalWeightReport == physicalWeightReport.Value);
            
            if (supplierId.HasValue)
                query = query.Where(cr => cr.SupplierId == supplierId.Value);

            if (productId.HasValue)
                query = query.Where(cr => cr.ProductId == productId.Value);
            
            if (createdById.HasValue)
                query = query.Where(cr => cr.CreatedById == createdById.Value);
            
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Актів виконаних робіт за параметрами", ex);
        }
    }
    
    public async Task<CompletionReport> UpdateCompletionReportAsync(CompletionReport completionReport, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            completionReport.ModifiedAt = DateTime.UtcNow;
            completionReport.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(completionReport, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Акта виконаних робіт з ID  {completionReport.Id}", ex);
        }
    }

    public async Task<CompletionReport> SoftDeleteCompletionReportAsync(CompletionReport completionReport, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            completionReport.RemovedAt = DateTime.UtcNow;
            completionReport.RemovedById = removedById;
            
            return await _repository.UpdateAsync(completionReport, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Акта виконаних робіт з ID  {completionReport.Id}", ex);
        }
    }
    
    public async Task<CompletionReport> RestoreRemovedCompletionReportAsync(CompletionReport completionReport, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            completionReport.RemovedAt = null;
            completionReport.RestoredAt = DateTime.UtcNow;
            completionReport.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(completionReport, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Акта виконаних робіт з ID  {completionReport.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteCompletionReportAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var CompletionReport = await _repository.GetByIdAsync<CompletionReport>(id, cancellationToken);
            if (CompletionReport != null)
            {
                await _repository.DeleteAsync<CompletionReport>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Акта виконаних робіт з ID {id}", ex);
        }
    }
}