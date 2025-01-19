using System.Security.Claims;
using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Security;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.DTOs.Requests;
using GrainElevatorAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/employee")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger, IRoleService roleService)
    {
        _employeeService = employeeService;
        _mapper = mapper;
        _logger = logger;
        _roleService = roleService;
    }

    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    [HttpGet("user-info")]
    [Authorize]
    public async Task<ActionResult> GetEmployeeInfo()
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized($"Співробітника з ID {userId} не авторізований.");
            }
            
            var employee  = await _employeeService.GetEmployeeByIdAsync(int.Parse(userId), cancellationToken);
            if (employee == null)
            {
                return NotFound($"Співробітника з ID {userId} не знайдено.");
            }
            
            var role = await _roleService.GetRoleByIdAsync(employee.RoleId, cancellationToken);
            if (role == null)
            {
                return BadRequest("Роль не знайдено.");
            }
            
            return Ok(new 
            {
                UserInfo = new 
                {
                    Id = employee.Id,
                    Name = $"{employee.FirstName} {employee.LastName}",
                    Role = role.Title
                }
            });

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin,HR,CEO")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int page = 1, int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var employees = await _employeeService.GetAllEmployeesAsync(page, size, cancellationToken);
            
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,HR,CEO")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
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
    [Authorize(Roles = "Admin,HR,CEO")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> SearchEmployees(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName,
        [FromQuery] string? roleTitle,
        [FromQuery] string? gender,
        [FromQuery] string? email,
        [FromQuery] string? phone,
        [FromQuery] string? city,
        [FromQuery] string? country,
        [FromQuery] DateTime? birthDate,
        [FromQuery] DateTime? lastSeenOnline,
        [FromQuery] string? createdByName,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? sortField = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var (filteredEmployees, totalCount) = await _employeeService.SearchEmployeesAsync(
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
                createdByName,
                page,
                size,
                sortField,
                sortOrder,
                cancellationToken);
           
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(filteredEmployees);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            
            return Ok(employeeDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Співробітника за назвою: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Співробітника за назвою: {ex.Message}");
        }
    }

   
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeUpdateRequest request)
    {
        // валідатор
        if (request.FirstName == null) ModelState.Remove("FirstName");
        if (request.LastName == null) ModelState.Remove("LastName");
        if (request.BirthDate == null) ModelState.Remove("BirthDate");
        if (request.Email == null) ModelState.Remove("Email");
        if (request.Phone == null) ModelState.Remove("Phone");
        if (request.City == null) ModelState.Remove("City");
        if (request.Country == null) ModelState.Remove("Country");
        if (request.PasswordHash == null) ModelState.Remove("PasswordHash");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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
            if (modifiedById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeDb, request.PasswordHash, request.RoleTitle, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<EmployeeDto>(updatedEmployee));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні Співробітника з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Співробітника з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpPut("{id}/last-seen-online")]
    [Authorize]
    public async Task<IActionResult> UpdateLastSeenOnlineEmployee(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var employeeDb = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);
            if (employeeDb == null)
            {
                return NotFound($"Співробітника з ID {id} не знайдено.");
            }
            
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (modifiedById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var updatedEmployee = await _employeeService.UpdateLastSeenOnlineEmployeeAsync(employeeDb, cancellationToken);
            
            return Ok(_mapper.Map<EmployeeDto>(updatedEmployee));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні часу останньої присутності Співробітника з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні часу останньої присутності Співробітника з ID {id}: {ex.Message}");
        }
    }
    
    [HttpPatch("{id}/soft-remove")]
    [Authorize(Roles = "Admin,HR")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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