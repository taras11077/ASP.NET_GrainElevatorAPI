using System.Linq.Expressions;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IEmployeeService
{
    Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<Employee> GetEmployeeByEmailAsync(string email);
    IEnumerable<Employee> GetAllEmployees(int page, int size);

    Task<Employee> UpdateEmployeeAsync(Employee employee, string passwordHash, int modifiedById, CancellationToken cancellationToken);
    
    Task<Employee> SoftDeleteEmployeeAsync(Employee employee, int removedById, CancellationToken cancellationToken);
    Task<Employee> RestoreRemovedEmployeeAsync(Employee employee, int restoredById, CancellationToken cancellationToken);
    
    Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetEmployeesByConditionAsync(Expression<Func<Employee, bool>> predicate);
}