using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IAuthService
{
    Task<Employee> Register(string email, string password, int roleId, CancellationToken cancellationToken);
    Task<Employee> Login(string email, string password, CancellationToken cancellationToken);
    Task<Employee?> FindByEmailAsync(string email, CancellationToken cancellationToken);
}