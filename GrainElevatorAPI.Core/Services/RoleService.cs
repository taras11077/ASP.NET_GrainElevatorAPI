using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class RoleService : IRoleService
{
    private readonly IRepository _repository;

    public RoleService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<Role> AddRoleAsync(string title)
    {
        try
        {
            var newRole = new Role{ Title = title };
            
            return await _repository.Add(newRole);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні ролі", ex);
        }
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        try
        {
            return await _repository.GetById<Role>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні ролі з ID {id}", ex);
        }
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        try
        {
            return await _repository.Update(role);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні ролі з ID  {role.Id}", ex);
        }
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        try
        {
            var role = await _repository.GetById<Role>(id);
            if (role != null)
            {
                await _repository.Delete<Role>(id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні ролі з ID {id}", ex);
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
}