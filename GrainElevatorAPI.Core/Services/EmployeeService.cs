using System.Linq.Expressions;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Security;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository _repository;
    //private readonly int roleCount = 6; //TODO

    public EmployeeService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<Employee>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні співробітника з ID {id}", ex);
        }
    }

    public async Task<Employee> GetEmployeeByEmailAsync(string email)
    {
        try
        {
            return await _repository.GetAll<Employee>()
                .Where(r => r.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Співробітника з електронною поштою {email}", ex);
        }
    }
    

    public IEnumerable<Employee> GetAllEmployees(int page, int size)
    {
        try
        {
            return _repository.GetAll<Employee>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку співробітників", ex);
        }
    }

    
    public async Task<Employee> UpdateEmployeeAsync(Employee employee, string passwordHash, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            employee.PasswordHash = passwordHash != null ? PasswordHasher.HashPassword(passwordHash) : employee.PasswordHash;
            employee.ModifiedAt = DateTime.UtcNow;
            employee.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні співробітника з ID  {employee.Id}", ex);
        }
    }
    
    public async Task<Employee> SoftDeleteEmployeeAsync(Employee employee, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            employee.RemovedAt = DateTime.UtcNow;
            employee.RemovedById = removedById;
            
            return await _repository.UpdateAsync(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Вхідної накладної з ID  {employee.Id}", ex);
        }
    }
    
    public async Task<Employee> RestoreRemovedEmployeeAsync(Employee employee, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            employee.RemovedAt = null;
            employee.RestoredAt = DateTime.UtcNow;
            employee.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при відновленні Вхідної накладної з ID  {employee.Id}", ex);
        }
    }

    public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _repository.GetByIdAsync<Employee>(id, cancellationToken);
            if (employee != null)
            {
                await _repository.DeleteAsync<Employee>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні співробітника з ID {id}", ex);
        }
        
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByConditionAsync(Expression<Func<Employee, bool>> predicate)
    {
        try
        {
            return await _repository.GetQuery(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка під час виконання пошуку співробітників", ex);
        }
    }
}