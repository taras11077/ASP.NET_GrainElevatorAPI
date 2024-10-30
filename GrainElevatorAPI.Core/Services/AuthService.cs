using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Security;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class AuthService : IAuthService
{
    private readonly IRepository _repository;

    public AuthService(IRepository repository)
    {
        _repository = repository;
    }
    
    // реєстрація співробітника   
    public async Task<Employee> Register(string email, string password, int roleId, CancellationToken cancellationToken)
    {
        // валідація вводу
        if (email == null || string.IsNullOrEmpty(email.Trim()) || email.Length < 4 ||
            password == null || string.IsNullOrEmpty(password.Trim()) || password.Length < 4 ||
            roleId <= 0)
        {
            throw new ArgumentException();
        }
        
        // перевірка існування користувача з таким самим нікнеймом
        if (_repository.GetAll<Employee>().Any(u => u.Email == email))
            throw new InvalidOperationException("Користувач з таким нікнеймом вже існує.");
    
        var hashedPassword = PasswordHasher.HashPassword(password);
    
        // створення нового співробітника
        var newEmployee = new Employee
        {
            Email = email,
            PasswordHash = hashedPassword,
            RoleId = roleId,
        };
    
        await _repository.AddAsync(newEmployee, cancellationToken);
    
        return newEmployee;
    }
    
    // логування співробітника      
    public async Task<Employee> Login(string email, string password, CancellationToken cancellationToken)
    {
        // валідація вводу
        if (email == null || string.IsNullOrEmpty(email.Trim()) || 
            password == null || string.IsNullOrEmpty(password.Trim()))
            throw new ArgumentNullException();
        
        // перевірка співробітника на наявність в базі
        var employee = _repository.GetAll<Employee>().FirstOrDefault(u => u.Email == email);
        if (employee == null)
            throw new UnauthorizedAccessException("Недійсний нікнейм.");
        
        // перевірка пароля
        if (!PasswordHasher.VerifyPassword(password, employee.PasswordHash))
            throw new UnauthorizedAccessException("Недійсний пароль.");
        
        // оновлення часу останнього відвідування
        employee.LastSeenOnline = DateTime.UtcNow;
        await _repository.UpdateAsync(employee, cancellationToken);
    
        return employee;
    }

    public async Task<Employee?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Employee>()
                .Where(r => r.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Співробітника з електронною поштою {email}", ex);
        }

    }
}