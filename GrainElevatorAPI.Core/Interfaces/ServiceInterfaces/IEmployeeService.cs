using System.Linq.Expressions;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IEmployeeService
{
    Task<Employee> GetEmployeeByIdAsync(int id);
    IEnumerable<Employee> GetAllEmployees(int page, int size);

    Task<Employee> UpdateEmployeeAsync(Employee employee, string passwordHash, int modifiedById);
    
    Task<Employee> SoftDeleteEmployeeAsync(Employee employee, int removedById);
    Task<Employee> RestoreRemovedEmployeeAsync(Employee employee, int restoredById);
    
    Task<bool> DeleteEmployeeAsync(int id);
    Task<IEnumerable<Employee>> GetEmployeesByConditionAsync(Expression<Func<Employee, bool>> predicate);
}