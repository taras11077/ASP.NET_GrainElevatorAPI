using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class TechnologicalOperationService: ITechnologicalOperationService
{
    private readonly IRepository _repository;

    public TechnologicalOperationService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<TechnologicalOperation> CreateTechnologicalOperationAsync(TechnologicalOperation technologicalOperation, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            technologicalOperation.CreatedAt = DateTime.UtcNow;
            technologicalOperation.CreatedById = createdById;
            
            return await _repository.AddAsync(technologicalOperation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при створенні Технологічної операції", ex);
        }
    }

    public async Task<TechnologicalOperation> GetTechnologicalOperationByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<TechnologicalOperation>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Технологічної операції з ID {id}", ex);
        }
    }
    
    public async Task<IEnumerable<TechnologicalOperation>> GetTechnologicalOperations(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<TechnologicalOperation>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Технологічних операцій", ex);
        }
    }
    
    
    public async Task<(IEnumerable<TechnologicalOperation>, int)> SearchTechnologicalOperationAsync(
        string? title,
        string? createdByName,
        int page,
        int size,
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<TechnologicalOperation>()
                .Where(to => to.RemovedAt == null);
            
            // Виклик методу фільтрації
            query = ApplyFilters(query, title, createdByName);

            // Виклик методу сортування
            query = ApplySorting(query, sortField, sortOrder);

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredOperations = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredOperations, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Технологічної операції", ex);
        }
    }
    
    private IQueryable<TechnologicalOperation> ApplyFilters(
        IQueryable<TechnologicalOperation> query,
        string? title,
        string? createdByName)
    {
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(to => to.Title == title);
        }
        if (!string.IsNullOrEmpty(createdByName))
        {
            query = query.Where(to => to.CreatedBy.LastName == createdByName);
        }
        
        return query;
    }
    
    private IQueryable<TechnologicalOperation> ApplySorting(
        IQueryable<TechnologicalOperation> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "title" => sortOrder == "asc"
                ? query.OrderBy(to => to.Title)
                : query.OrderByDescending(to => to.Title),
            "createdByName" => sortOrder == "asc"
                ? query.OrderBy(to => to.CreatedBy.LastName)
                : query.OrderByDescending(to => to.CreatedBy.LastName),
            _ => query // Якщо поле не визначене
        };
    }

    public async Task<TechnologicalOperation> UpdateTechnologicalOperationAsync(TechnologicalOperation technologicalOperation, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            technologicalOperation.ModifiedAt = DateTime.UtcNow;
            technologicalOperation.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(technologicalOperation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Технологічної операції з ID  {technologicalOperation.Id}", ex);
        }
    }

    public async Task<TechnologicalOperation> SoftDeleteTechnologicalOperationAsync(TechnologicalOperation technologicalOperation, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            technologicalOperation.RemovedAt = DateTime.UtcNow;
            technologicalOperation.RemovedById = removedById;
            
            return await _repository.UpdateAsync(technologicalOperation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Технологічної операції з ID  {technologicalOperation.Id}", ex);
        }
    }
    
    public async Task<TechnologicalOperation> RestoreRemovedTechnologicalOperationAsync(TechnologicalOperation technologicalOperation, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            technologicalOperation.RemovedAt = null;
            technologicalOperation.RestoredAt = DateTime.UtcNow;
            technologicalOperation.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(technologicalOperation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Технологічної операції з ID  {technologicalOperation.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteTechnologicalOperationAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var technologicalOperation = await _repository.GetByIdAsync<TechnologicalOperation>(id, cancellationToken);
            if (technologicalOperation != null)
            {
                await _repository.DeleteAsync<TechnologicalOperation>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Технологічної операції з ID {id}", ex);
        }
    }
    
}