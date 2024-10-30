using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class RoleService : IRoleService
{
    private readonly IRepository _repository;

    public RoleService(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Role> CreateRoleAsync(Role role, CancellationToken cancellationToken)
    {
        try
        {
            role.CreatedAt = DateTime.UtcNow;
            return await _repository.AddAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при створенні ролі", ex);
        }
    }
    
    
    public async Task<Role> AddRoleAsync(Role role, int? createdById, CancellationToken cancellationToken)
    {
        try
        {
            role.CreatedAt = DateTime.UtcNow;
            
            if (createdById.HasValue)
                role.CreatedById = createdById;
            
            return await _repository.AddAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні ролі", ex);
        }
    }

    public async Task<Role> GetRoleByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<Role>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Ролі з ID {id}", ex);
        }
    }

    public async Task<Role> GetRoleByTitleAsync(string title)
    {
        try
        {
            return await _repository.GetAll<Role>()
                .Where(r => r.Title.ToLower() == title.ToLower())
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Ролі з назвою {title}", ex);
        }
    }

    public IEnumerable<Role> GetRoles(int page, int size)
    {
        try
        {
            return _repository.GetAll<Role>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку ролів", ex);
        }
    }

    public IEnumerable<Role> SearchRoles(string title)
    {
        try
        {
            return _repository.GetAll<Role>()
                .Where(r => r.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні ролі з назвою {title}", ex);
        }
    }
    
    public async Task<Role> UpdateRoleAsync(Role role, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            role.ModifiedAt = DateTime.UtcNow;
            role.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні ролі з ID  {role.Id}", ex);
        }
    }
    
    public async Task<Role> SoftDeleteRoleAsync(Role role, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            role.RemovedAt = DateTime.UtcNow;
            role.RemovedById = removedById;
            
            return await _repository.UpdateAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Продукції з ID  {role.Id}", ex);
        }
    }
    
    public async Task<Role> RestoreRemovedRoleAsync(Role role, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            role.RemovedAt = null;
            role.RestoredAt = DateTime.UtcNow;
            role.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при відновленні Продукції з ID  {role.Id}", ex);
        }
    }

    public async Task<bool> DeleteRoleAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _repository.GetByIdAsync<Role>(id, cancellationToken);
            if (role != null)
            {
                await _repository.DeleteAsync<Role>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Ролі з ID {id}", ex);
        }
    }
    
}