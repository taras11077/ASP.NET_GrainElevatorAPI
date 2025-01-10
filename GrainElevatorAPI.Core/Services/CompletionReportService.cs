using GrainElevatorAPI.Core.Calculators;
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
        
        if (registers == null || !registers.Any())
        {
            throw new ArgumentNullException("Не знайдено жодного Реєстру для обробки.");
        }
        
        var operations = await _repository.GetAll<TechnologicalOperation>()
            .Where(op => operationIds.Contains(op.Id))
            .ToListAsync(cancellationToken);
        
        if (operations == null || !operations.Any())
        {
            throw new ArgumentNullException("Не знайдено жодної Технологичної операції для обробки.");
        }
        
        var productId = registers.First().ProductId;
        var supplierId = registers.First().SupplierId;
        
        var completionReport = new CompletionReport
        {
            ReportNumber = reportNumber,
            ReportDate = DateTime.UtcNow,
            ProductId = productId,
            SupplierId = supplierId,
            CreatedAt = DateTime.UtcNow,
            CreatedById = createdById
        };

        // використання калькулятора для обчислення вагових характеристик
        var сompletionReportCalculator = new CompletionReportCalculator();
        сompletionReportCalculator.CalculateWeights(registers, completionReport);

        // Встановлення IsFinalized = true для кожного Реєстра
        foreach (var register in registers)
        {
            register.IsFinalized = true;
            await _repository.UpdateAsync(register, cancellationToken);
        }

        // додавання операцій до Акта
        foreach (var operation in operations)
        {
            var amount = сompletionReportCalculator.MapOperationToReportField(operation, completionReport) ?? 0;

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
    catch (ArgumentNullException ex)
    {
        throw;
    }
    catch (Exception ex)
    {
        throw new Exception("Помилка сервісу при створенні Акта виконаних робіт", ex);
    }
}
    
    public async Task<CompletionReport> CalculateReportCostAsync(
        int reportId, 
        int priceListId,
        int modifiedById,
        CancellationToken cancellationToken)
    {
        try
        {
            var completionReport = await _repository.GetByIdAsync<CompletionReport>(reportId, cancellationToken);
            if (completionReport == null)
                throw new Exception($"Акт виконаних робіт з ID {reportId} не знайдено");

            var priceList = await _repository.GetByIdAsync<PriceList>(priceListId, cancellationToken);
            if (priceList == null)
                throw new Exception($"Прайс-лист з ID {priceListId} не знайдено");

            var сompletionReportCalculator = new CompletionReportCalculator();
            сompletionReportCalculator.CalculateTotalCost(completionReport, priceList);

            completionReport.ModifiedAt = DateTime.UtcNow;
            completionReport.ModifiedById = modifiedById;
        
            await _repository.SaveChangesAsync(cancellationToken);

            return completionReport;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при обчисленні вартості акта виконаних робіт", ex);
        }
       
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

     public async Task<(IEnumerable<CompletionReport>, int)> SearchCompletionReportsAsync(
         string? reportNumber = null,
         DateTime? reportDate = null,
         double? physicalWeightReport =null,
         decimal? totalCost = null,
         string? supplierTitle = null,
         string? productTitle = null,
         string? createdByName = null,
         int page = 1,
         int size = 10,
         string? sortField = null,
         string? sortOrder = null,
         CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _repository.GetAll<CompletionReport>()
                .Where(ir => ir.RemovedAt == null);
            
            // Виклик методу фільтрації
            query = ApplyFilters(query, reportNumber, reportDate, physicalWeightReport, 
                totalCost, supplierTitle, productTitle, createdByName);

            // Виклик методу сортування
            query = ApplySorting(query, sortField, sortOrder);

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredReports = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredReports, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Актів виконаних робіт за параметрами", ex);
        }
    }
     
     
       private IQueryable<CompletionReport> ApplyFilters(
        IQueryable<CompletionReport> query,
        string? reportNumber,
        DateTime? reportDate,
        double? physicalWeightReport,
        decimal? totalCost,
        string? supplierTitle,
        string? productTitle,
        string? createdByName)
    {
        if (!string.IsNullOrEmpty(reportNumber))
        {
            query = query.Where(cr => cr.ReportNumber == reportNumber);
        }
            
        if (reportDate.HasValue)
        {
            query = query.Where(cr => cr.ReportDate.Date == reportDate.Value.Date);
        }
        
        if (physicalWeightReport.HasValue)
            query = query.Where(cr => cr.PhysicalWeightReport == physicalWeightReport.Value);
        
        if (totalCost.HasValue)
            query = query.Where(cr => cr.TotalCost == totalCost.Value);
            
        if (!string.IsNullOrEmpty(supplierTitle))
        {
            query = query.Where(cr => cr.Supplier.Title == supplierTitle);
        }

        if (!string.IsNullOrEmpty(productTitle))
        {
            query = query.Where(cr => cr.Product.Title == productTitle);
        }
            
        if (!string.IsNullOrEmpty(createdByName))
        {
            query = query.Where(cr => cr.CreatedBy.LastName == createdByName);
        }
        
        return query;
    }

    
    private IQueryable<CompletionReport> ApplySorting(
        IQueryable<CompletionReport> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "reportNumber" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.ReportNumber)
                : query.OrderByDescending(reg => reg.ReportNumber),
            "reportDate" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.ReportDate)
                : query.OrderByDescending(reg => reg.ReportDate),
            "productTitle" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.Product.Title)
                : query.OrderByDescending(reg => reg.Product.Title),
            "supplierTitle" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.Supplier.Title)
                : query.OrderByDescending(reg => reg.Supplier.Title),
            "physicalWeightReport" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.PhysicalWeightReport)
                : query.OrderByDescending(reg => reg.PhysicalWeightReport),
            "accWeightReport" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.AccWeightReport)
                : query.OrderByDescending(reg => reg.AccWeightReport),
            "wasteReport" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.WasteReport)
                : query.OrderByDescending(reg => reg.WasteReport),
            "quantitiesDryingReport" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.QuantitiesDryingReport)
                : query.OrderByDescending(reg => reg.QuantitiesDryingReport),
            "totalCost" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.TotalCost)
                : query.OrderByDescending(reg => reg.TotalCost),
            
            "createdByName" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.CreatedBy.LastName)
                : query.OrderByDescending(reg => reg.CreatedBy.LastName),
            _ => query // Якщо поле не визначене
        };
    }
    
    public async Task<CompletionReport> UpdateCompletionReportAsync(
        int id,
        string? reportNumber,
        DateTime? reportDate,
        int modifiedById, 
        CancellationToken cancellationToken)
    {
        try
        {
            var completionReportDb = await _repository.GetByIdAsync<CompletionReport>(id, cancellationToken)
                                     ?? throw new InvalidOperationException($"CompletionReport with ID {id} not found.");
            
            if (reportNumber != null || reportDate != null)
            {
                completionReportDb.ReportNumber = reportNumber ?? completionReportDb.ReportNumber;
                completionReportDb.ReportDate = reportDate ?? completionReportDb.ReportDate;
                completionReportDb.ModifiedById = modifiedById;
                completionReportDb.ModifiedAt = DateTime.UtcNow;
            }
            
            return await _repository.UpdateAsync(completionReportDb, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу під час оновлення Акта виконаних робіт з ID {id}", ex);
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