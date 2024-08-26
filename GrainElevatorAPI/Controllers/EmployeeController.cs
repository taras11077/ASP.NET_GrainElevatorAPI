using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Security;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Extensions;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[Route("api/employee")]
[ApiController]
[Authorize(Roles = "admin")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    // GET: api/Employee
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int page = 1, int size = 10)
    {
        try
        {
            var employees = _employeeService.GetAllEmployees(page, size);
            return Ok(_mapper.Map<IEnumerable<EmployeeDTO>>(employees));
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // GET: api/Employee/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound($"Співробітник з ID {id} не знайдений.");
            }
            return Ok(_mapper.Map<EmployeeDTO>(employee));
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // PUT: api/Employee/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeUpdateRequest request)
    {
        try
        {
            var employeeDb = await _employeeService.GetEmployeeById(id);
            
            employeeDb.UpdateFromRequest(request);
            
            employeeDb.PasswordHash = request.PasswordHash != null ? PasswordHasher.HashPassword(request.PasswordHash) : employeeDb.PasswordHash;
            
            var updatedEmployee = await _employeeService.UpdateEmployee(employeeDb);
            
            if (updatedEmployee == null)
            {
                return NotFound($"Співробітник з ID {id} не знайдений.");
            }

            return Ok(_mapper.Map<EmployeeDTO>(updatedEmployee));
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // DELETE: api/Employee/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            var success = await _employeeService.DeleteEmployee(id);
            if (!success)
            {
                return NotFound($"Співробітник з ID {{id}} не знайдений.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // GET: api/Employee/search?name=Vasyl
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployees(string name)
    {
        try
        {
            var employees = await _employeeService.GetEmployeesByCondition(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            return Ok(_mapper.Map<IEnumerable<EmployeeDTO>>(employees));
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
}