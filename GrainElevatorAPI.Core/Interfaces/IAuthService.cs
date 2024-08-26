using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IAuthService
{
    Task<Employee> Register(string email, string password, int roleId);
    Task<Employee> Login(string email, string password);
}