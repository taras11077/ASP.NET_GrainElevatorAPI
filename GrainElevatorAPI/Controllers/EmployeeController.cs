using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Security;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Extensions;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/employee")]

//[Authorize(Roles = "admin")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }


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
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }
            return Ok(_mapper.Map<EmployeeDTO>(employee));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, EmployeeUpdateRequest request)
    {
        try
        {
            var employeeDb = await _employeeService.GetEmployeeByIdAsync(id);
            if (employeeDb == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }
            
            employeeDb.UpdateFromRequest(request);
            
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeDb, request.PasswordHash, modifiedById);
            
            return Ok(_mapper.Map<EmployeeDTO>(updatedEmployee));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> SoftDeleteEmployee(int id)
    {
        try
        {
            var employeeDb = await _employeeService.GetEmployeeByIdAsync(id);
            if (employeeDb == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }

            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedEmployee = await _employeeService.SoftDeleteEmployeeAsync(employeeDb, removedById);
            
            return Ok(_mapper.Map<EmployeeDTO>(removedEmployee));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> RestoreRemovedEmployee(int id)
    {
        try
        {
            var employeeDb = await _employeeService.GetEmployeeByIdAsync(id);
            if (employeeDb == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredEmployee = await _employeeService.RestoreRemovedEmployeeAsync(employeeDb, restoredById);

            return Ok(_mapper.Map<EmployeeDTO>(restoredEmployee));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            var success = await _employeeService.DeleteEmployeeAsync(id);
            if (!success)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployees(string name)
    {
        try
        {
            var employees = await _employeeService.GetEmployeesByConditionAsync(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            return Ok(_mapper.Map<IEnumerable<EmployeeDTO>>(employees));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
}