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
            throw new Exception("Помилка сервісу при створенні Ролі", ex);
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
            throw new Exception("Помилка сервісу при додаванні Ролі", ex);
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
            throw new Exception($"Помилка сервісу при отриманні Ролі з ID {id}", ex);
        }
    }

    public async Task<Role?> GetRoleByTitleAsync(string title, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Role>()
                .Where(r => r.Title.ToLower() == title.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Ролі з назвою {title}", ex);
        }
    }

    public async Task<IEnumerable<Role>> GetRoles(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Role>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Ролів", ex);
        }
    }

    public async Task<IEnumerable<Role>> SearchRoles(string title, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Role>()
                .Where(r => r.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Ролі з назвою {title}", ex);
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
            throw new Exception($"Помилка сервісу при оновленні Ролі з ID  {role.Id}", ex);
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
            throw new Exception($"Помилка сервісу при видаленні Ролі з ID  {role.Id}", ex);
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
            throw new Exception($"Помилка сервісу при відновленні Ролі з ID  {role.Id}", ex);
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
            throw new Exception($"Помилка сервісу при видаленні Ролі з ID {id}", ex);
        }
    }
    
}