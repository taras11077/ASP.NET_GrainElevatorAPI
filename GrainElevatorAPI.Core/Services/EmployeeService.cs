﻿using System.Linq.Expressions;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Security;

namespace GrainElevatorAPI.Core.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository _repository;
    private readonly int roleCount = 6; //TODO

    public EmployeeService(IRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<Employee> GetEmployeeById(int id)
    {
        try
        {
            return await _repository.GetById<Employee>(id);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            throw new Exception($"Помилка при отриманні співробітника з ID {id}", ex);
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
            // Логирование ошибки
            throw new Exception("Помилка при отриманні списку співробітників", ex);
        }
    }

    
    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        try
        {
            return await _repository.Update(employee);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            throw new Exception($"Помилка при оновленні співробітника з ID  {employee.Id}", ex);
        }
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        try
        {
            var employee = await _repository.GetById<Employee>(id);
            if (employee != null)
            {
                await _repository.Delete<Employee>(id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            throw new Exception($"Помилка при видаленні співробітника з ID {id}", ex);
        }
        
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByCondition(Expression<Func<Employee, bool>> predicate)
    {
        try
        {
            return await _repository.GetQuery(predicate);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            throw new Exception("Помилка під час виконання пошуку співробітників", ex);
        }
    }
}