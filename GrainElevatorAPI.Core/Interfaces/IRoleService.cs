using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IRoleService
{
    Task<Role> AddRole(string  title);
    Task<Role> GetRoleById(int id);
    Task<Role> UpdateRole(Role role);
    
    Task<bool> DeleteRole(int id);
    
    IEnumerable<Role> GetRoles(int page, int size);
    IEnumerable<Role> SearchRoles(string title);
}