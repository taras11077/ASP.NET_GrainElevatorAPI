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
    
    
    public async Task<IEnumerable<TechnologicalOperation>> SearchTechnologicalOperation(string title, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<TechnologicalOperation>()
                .Where(s => s.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Технологічної операції з назвою {title}", ex);
        }
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