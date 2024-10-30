using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IRoleService
{
    Task<Role> CreateRoleAsync(Role role, CancellationToken cancellationToken);
    Task<Role> AddRoleAsync(Role role, int? createdById, CancellationToken cancellationToken);
    Task<Role> GetRoleByIdAsync(int id, CancellationToken cancellationToken);
    Task<Role> GetRoleByTitleAsync(string title, CancellationToken cancellationToken);
    Task<IEnumerable<Role>> GetRoles(int page, int size, CancellationToken cancellationToken);
    Task<IEnumerable<Role>> SearchRoles(string title, CancellationToken cancellationToken);
    Task<Role> UpdateRoleAsync(Role role, int modifiedById, CancellationToken cancellationToken);
    
    Task<Role> SoftDeleteRoleAsync(Role role, int removedById, CancellationToken cancellationToken);
    Task<Role> RestoreRemovedRoleAsync(Role role, int restoredById, CancellationToken cancellationToken);
    
    Task<bool> DeleteRoleAsync(int id, CancellationToken cancellationToken);
}