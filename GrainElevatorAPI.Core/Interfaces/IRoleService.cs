using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IRoleService
{
    Task<Role> AddRoleAsync(string  title);
    Task<Role> GetRoleByIdAsync(int id);
    Task<Role> UpdateRoleAsync(Role role);
    
    Task<bool> DeleteRoleAsync(int id);
    
    IEnumerable<Role> GetRoles(int page, int size);
    IEnumerable<Role> SearchRoles(string title);
}