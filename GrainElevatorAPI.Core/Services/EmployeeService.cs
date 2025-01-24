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

    public async Task<(IEnumerable<Employee>, int)> SearchEmployeesAsync(
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
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _repository.GetAll<Employee>()
                .Include(cr => cr.Role)
                .Include(e => e.CreatedBy)
                .Where(e => e.RemovedAt == null);
            
            // Виклик методу фільтрації
            query = ApplyFilters(
                            query, 
                            firstName, 
                            lastName, 
                            roleTitle,
                            gender, 
                            email, 
                            phone, 
                            city,
                            country,
                            birthDate,
                            lastSeenOnline,
                            createdByName);

            // Виклик методу сортування
            query = ApplySorting(query, sortField, sortOrder);

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredEmployees = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredEmployees, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Актів виконаних робіт за параметрами", ex);
        }
    }
    
    
      private IQueryable<Employee> ApplyFilters(
        IQueryable<Employee> query,
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
        string? createdByName)
    {
        if (!string.IsNullOrEmpty(firstName))
        {
            query = query.Where(e => e.FirstName == firstName);
        }
        
        if (!string.IsNullOrEmpty(lastName))
        {
            query = query.Where(e => e.LastName == lastName);
        }

        if (!string.IsNullOrEmpty(roleTitle))
        {
            query = query.Where(e => e.Role.Title == roleTitle);
        }
        
        if (!string.IsNullOrEmpty(gender))
        {
            query = query.Where(e => e.Gender == gender);
        }
        
        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(e => e.Email == email);
        }
        
        if (!string.IsNullOrEmpty(phone))
        {
            query = query.Where(e => e.Phone == phone);
        }
        
        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(e => e.City == city);
        }

        if (!string.IsNullOrEmpty(country))
        {
            query = query.Where(e => e.Country == country);
        }
        
        if (birthDate.HasValue)
        {
            query = query.Where(e => e.BirthDate == birthDate.Value.Date);
        }
        
        if (lastSeenOnline.HasValue)
        {
            query = query.Where(e => e.LastSeenOnline.Date == lastSeenOnline.Value.Date);
        }
            
        if (!string.IsNullOrEmpty(createdByName))
        {
            query = query.Where(cr => cr.CreatedBy.LastName == createdByName);
        }
        
        return query;
    }

    
    private IQueryable<Employee> ApplySorting(
        IQueryable<Employee> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "id" => sortOrder == "asc"
                ? query.OrderBy(e => e.Id)
                : query.OrderByDescending(e => e.Id),
            "firstName" => sortOrder == "asc"
                ? query.OrderBy(e => e.FirstName)
                : query.OrderByDescending(e => e.FirstName),
            "lastName" => sortOrder == "asc"
                ? query.OrderBy(e => e.LastName)
                : query.OrderByDescending(e => e.LastName),
            "roleTitle" => sortOrder == "asc"
                ? query.OrderBy(e => e.Role.Title)
                : query.OrderByDescending(e => e.Role.Title),
            "gender" => sortOrder == "asc"
                ? query.OrderBy(e => e.Gender)
                : query.OrderByDescending(e => e.Gender),
            "email" => sortOrder == "asc"
                ? query.OrderBy(e => e.Email)
                : query.OrderByDescending(e => e.Email),
            "phone" => sortOrder == "asc"
                ? query.OrderBy(e => e.FirstName)
                : query.OrderByDescending(e => e.FirstName),
            "city" => sortOrder == "asc"
                ? query.OrderBy(e => e.LastName)
                : query.OrderByDescending(e => e.LastName),
            "country" => sortOrder == "asc"
                ? query.OrderBy(e => e.Gender)
                : query.OrderByDescending(e => e.Gender),
            "birthDate" => sortOrder == "asc"
                ? query.OrderBy(e => e.BirthDate)
                : query.OrderByDescending(e => e.BirthDate),
            "lastSeenOnline" => sortOrder == "asc"
                ? query.OrderBy(e => e.LastSeenOnline)
                : query.OrderByDescending(e => e.LastSeenOnline),
            "createdByName" => sortOrder == "asc"
                ? query.OrderBy(reg => reg.CreatedBy.LastName)
                : query.OrderByDescending(reg => reg.CreatedBy.LastName),
            _ => query // Якщо поле не визначене
        };
    }
    
    public async Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<Employee>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні співробітника з ID {id}", ex);
        }
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Employee>()
                .Where(r => r.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Співробітника з електронною поштою {email}", ex);
        }
    }
    

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Employee>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку співробітників", ex);
        }
    }

    
    public async Task<Employee> UpdateEmployeeAsync(
        Employee employee,
        string? passwordHash,
        string? roleTitle, 
        int modifiedById, 
        CancellationToken cancellationToken)
    {
        try
        {
            var role = await _repository.GetAll<Role>()
                .FirstOrDefaultAsync(r => r.Title == roleTitle, cancellationToken);
        
            employee.RoleId = role?.Id ?? employee.RoleId;
            employee.PasswordHash = passwordHash != null ? PasswordHasher.HashPassword(passwordHash) : employee.PasswordHash;
            employee.ModifiedAt = DateTime.UtcNow;
            employee.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Співробітника з ID  {employee.Id}", ex);
        }
    }

    public async Task<Employee> UpdateLastSeenOnlineEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        try
        {
            employee.LastSeenOnline = DateTime.UtcNow;
            
            return await _repository.UpdateAsync(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні часу останньої присутності Співробітника з ID  {employee.Id}", ex);
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
            throw new Exception($"Помилка сервісу при видаленні Співробітника з ID  {employee.Id}", ex);
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
            throw new Exception($"Помилка сервісу при відновленні Співробітника з ID  {employee.Id}", ex);
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
            throw new Exception($"Помилка сервісу при видаленні Співробітника з ID {id}", ex);
        }
        
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByConditionAsync(Expression<Func<Employee, bool>> predicate, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetQuery(predicate).ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу під час виконання пошуку Співробітників", ex);
        }
    }
}