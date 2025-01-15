using System.Linq.Expressions;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IEmployeeService
{
    Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync(int page, int size, CancellationToken cancellationToken);

    Task<(IEnumerable<Employee>, int)> SearchEmployeesAsync(
        string? firstName,
        string? lastName,
        string? roleTitle,
        string? gender,
        string? email,
        string? phone,
        string? city,
        string? country,
        DateTime? birthDate,
        DateTime? lastSeenOnline,
        string? createdByName,
        int page = 1,
        int size = 10,
        string? sortField = null,
        string? sortOrder = null,
        CancellationToken cancellationToken = default);
    
    Task<Employee> UpdateEmployeeAsync(
        Employee employee,
        string? passwordHash,
        string? roleTitle, 
        int modifiedById, 
        CancellationToken cancellationToken);
    
    Task<Employee> UpdateLastSeenOnlineEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    
    Task<Employee> SoftDeleteEmployeeAsync(Employee employee, int removedById, CancellationToken cancellationToken);
    Task<Employee> RestoreRemovedEmployeeAsync(Employee employee, int restoredById, CancellationToken cancellationToken);
    
    Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetEmployeesByConditionAsync(Expression<Func<Employee, bool>> predicate, CancellationToken cancellationToken);
}