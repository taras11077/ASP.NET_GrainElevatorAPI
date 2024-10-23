using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IRoleService
{
    Task<Role> AddRoleAsync(Role role, int createdById);
    Task<Role> GetRoleByIdAsync(int id);
    Task<Role> UpdateRoleAsync(Role role, int modifiedById);
    
    Task<Role> SoftDeleteRoleAsync(Role role, int removedById);
    Task<Role> RestoreRemovedRoleAsync(Role role, int restoredById);
    
    Task<bool> DeleteRoleAsync(int id);
    
    IEnumerable<Role> GetRoles(int page, int size);
    IEnumerable<Role> SearchRoles(string title);
}