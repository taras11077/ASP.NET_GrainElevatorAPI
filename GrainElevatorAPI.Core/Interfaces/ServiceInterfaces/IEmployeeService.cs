using System.Linq.Expressions;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IEmployeeService
{
    Task<Employee> GetEmployeeById(int id);
    IEnumerable<Employee> GetAllEmployees(int page, int size);
    
    Task<Employee> UpdateEmployee(Employee employee);
    
    Task<bool> DeleteEmployee(int id);
    Task<IEnumerable<Employee>> GetEmployeesByCondition(Expression<Func<Employee, bool>> predicate);
}