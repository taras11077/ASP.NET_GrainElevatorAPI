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
    
    public async Task<Role> CreateRoleAsync(Role role)
    {
        try
        {
            role.CreatedAt = DateTime.UtcNow;
            
            await _repository.AddAsync(role);
            await _repository.SaveChangesAsync();
            
            return role;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при створенні ролі", ex);
        }
    }
    
    
    public async Task<Role> AddRoleAsync(Role role, int? createdById)
    {
        try
        {
            role.CreatedAt = DateTime.UtcNow;
            
            if (createdById.HasValue)
                role.CreatedById = createdById;
            
            await _repository.AddAsync(role);
            await _repository.SaveChangesAsync();
            
            return role;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при додаванні Ролі", ex);
        }
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync<Role>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Ролі з ID {id}", ex);
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
            throw new Exception($"Помилка сервісу при отриманні Ролі з назвою {title}", ex);
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
            throw new Exception("Помилка сервісу при отриманні списку Ролів", ex);
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
            throw new Exception($"Помилка сервісу при отриманні Ролі з назвою {title}", ex);
        }
    }
    
    public async Task<Role> UpdateRoleAsync(Role role, int modifiedById)
    {
        try
        {
            role.ModifiedAt = DateTime.UtcNow;
            role.ModifiedById = modifiedById;
 
            await _repository.UpdateAsync(role);
            await _repository.SaveChangesAsync();
            
            return role;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Ролі з ID  {role.Id}", ex);
        }
    }
    
    public async Task<Role> SoftDeleteRoleAsync(Role role, int removedById)
    {
        try
        {
            role.RemovedAt = DateTime.UtcNow;
            role.RemovedById = removedById;
            
            await _repository.UpdateAsync(role);
            await _repository.SaveChangesAsync();
            
            return role;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Ролі з ID  {role.Id}", ex);
        }
    }
    
    public async Task<Role> RestoreRemovedRoleAsync(Role role, int restoredById)
    {
        try
        {
            role.RemovedAt = null;
            role.RestoredAt = DateTime.UtcNow;
            role.RestoreById = restoredById;
            
            await _repository.UpdateAsync(role);
            await _repository.SaveChangesAsync();
            
            return role;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Ролі з ID  {role.Id}", ex);
        }
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        try
        {
            var role = await _repository.GetByIdAsync<Role>(id);
            if (role != null)
            {
                await _repository.DeleteAsync<Role>(id);
                await _repository.SaveChangesAsync();
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