using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class CompletionReportOperationService: ICompletionReportOperationService
{
    private readonly IRepository _repository;

    public CompletionReportOperationService(IRepository repository) => _repository = repository;


    public async Task<CompletionReportOperation> CreateCompletionReportOperationAsync(CompletionReportOperation completionReportOperation, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            completionReportOperation.CreatedAt = DateTime.UtcNow;
            completionReportOperation.CreatedById = createdById;
            
            return await _repository.AddAsync(completionReportOperation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при створенні Операції акта виконаних робіт", ex);
        }
    }
    public async Task<IEnumerable<CompletionReportOperation>> GetCompletionReportOperations(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<CompletionReportOperation>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Операцій акта виконаних робіт", ex);
        }
    }
    public async Task<CompletionReportOperation> GetCompletionReportOperationByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<CompletionReportOperation>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Операції акта виконаних робіт з ID {id}", ex);
        }
    }

    public async Task<IEnumerable<CompletionReportOperation>> SearchCompletionReportOperations(int? id,
         int? technologicalOperationId,
         double? amount,
         int? completionReportId,
         int? createdById,
         int page,
         int size,
         CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<CompletionReportOperation>()
                .Skip((page - 1) * size)
                .Take(size);

            if (id.HasValue)
            {
                query = query.Where(cro => cro.Id == id);
            }
            
            if (technologicalOperationId.HasValue)
                query = query.Where(cro => cro.TechnologicalOperationId == technologicalOperationId.Value);
            
            if (amount.HasValue)
                query = query.Where(cro => cro.Amount == amount.Value);
            
            if (completionReportId.HasValue)
                query = query.Where(cro => cro.CompletionReportId == completionReportId.Value);
            
            if (createdById.HasValue)
                query = query.Where(cro => cro.CreatedById == createdById.Value);
            
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Операцій акта виконаних робіт за параметрами", ex);
        }
    }
    
    public async Task<CompletionReportOperation> UpdateCompletionReportOperationAsync(CompletionReportOperation completionReportOperation, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            completionReportOperation.ModifiedAt = DateTime.UtcNow;
            completionReportOperation.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(completionReportOperation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Операції акта виконаних робіт з ID  {completionReportOperation.Id}", ex);
        }
    }

    public async Task<CompletionReportOperation> SoftDeleteCompletionReportOperationAsync(CompletionReportOperation completionReportOperation, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            completionReportOperation.RemovedAt = DateTime.UtcNow;
            completionReportOperation.RemovedById = removedById;
            
            return await _repository.UpdateAsync(completionReportOperation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Операції акта виконаних робіт з ID  {completionReportOperation.Id}", ex);
        }
    }
    
    public async Task<CompletionReportOperation> RestoreRemovedCompletionReportOperationAsync(CompletionReportOperation completionReportOperation, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            completionReportOperation.RemovedAt = null;
            completionReportOperation.RestoredAt = DateTime.UtcNow;
            completionReportOperation.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(completionReportOperation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Операції акта виконаних робіт з ID  {completionReportOperation.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteCompletionReportOperationAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var CompletionReportOperation = await _repository.GetByIdAsync<CompletionReportOperation>(id, cancellationToken);
            if (CompletionReportOperation != null)
            {
                await _repository.DeleteAsync<CompletionReportOperation>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Операції акта виконаних робіт з ID {id}", ex);
        }
    }
}