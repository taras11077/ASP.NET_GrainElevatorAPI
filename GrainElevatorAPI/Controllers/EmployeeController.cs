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
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _mapper = mapper;
        _logger = logger;
    }

    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int page = 1, int size = 10)
    {
        try
        {
            var employees = _employeeService.GetAllEmployees(page, size);
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
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
            var cancellationToken = GetCancellationToken();
            var employee = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);
            if (employee == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }
            return Ok(_mapper.Map<EmployeeDto>(employee));
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
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Співробітника за назвою: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Співробітника за назвою: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeUpdateRequest request)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var employeeDb = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);
            if (employeeDb == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }
            
            employeeDb.UpdateFromRequest(request);
            
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeDb, request.PasswordHash, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<EmployeeDto>(updatedEmployee));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні Співробітника з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Співробітника з ID {id}: {ex.Message}");
        }
    }
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> SoftDeleteEmployee(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var employeeDb = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);
            if (employeeDb == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }

            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedEmployee = await _employeeService.SoftDeleteEmployeeAsync(employeeDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<EmployeeDto>(removedEmployee));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Співробітника з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Співробітника з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> RestoreRemovedEmployee(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var employeeDb = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);
            if (employeeDb == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredEmployee = await _employeeService.RestoreRemovedEmployeeAsync(employeeDb, restoredById, cancellationToken);

            return Ok(_mapper.Map<EmployeeDto>(restoredEmployee));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Співробітника з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Співробітника з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _employeeService.DeleteEmployeeAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Співробітника з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Співробітника з ID {id}: {ex.Message}");
        }
    }
    
}