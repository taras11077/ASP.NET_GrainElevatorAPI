using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GrainElevatorAPI.Controllers;

//[Authorize(Roles = "admin")]
[ApiController]
[Route("api/role")]
public class RoleController : ControllerBase
{
   private readonly IRoleService _roleService;
   private readonly IMapper _mapper;
   private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, IMapper mapper, ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _mapper = mapper;
        _logger = logger;
    }
    
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole(RoleCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (createdById <= 0)
                return Unauthorized(new { message = "Користувач не авторизований." });
            
            var newRole = _mapper.Map<Role>(request);
            
            var createdRole = await _roleService.AddRoleAsync(newRole, createdById, cancellationToken);
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, _mapper.Map<RoleDto>(createdRole));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Ролі: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Ролі: {ex.Message}");
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var roles = await _roleService.GetRolesAsync(page, size, cancellationToken);
            
            var rolesDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return Ok(rolesDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Ролей: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Ролей: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetRoleById(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var role = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (role == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            
            var roleDto = _mapper.Map<RoleDto>(role);
            return Ok(roleDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Ролі з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Ролі з ID {id}: {ex.Message}");
        }
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> SearchRoles(
        [FromQuery] string? title,
        [FromQuery] string? createdByName,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? sortField = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var (roles, totalCount) = await _roleService.SearchRolesAsync(
                title,
                createdByName,
                page, 
                size,
                sortField, sortOrder,
                cancellationToken);
            
            var rolesDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            
            return Ok(rolesDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Ролі за назвою: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Ролі за назвою: {ex.Message}");
        }
    } 
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, RoleCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (modifiedById <= 0)
                return Unauthorized(new { message = "Користувач не авторизований." });
            
            var roleDb = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (roleDb == null)
                return NotFound($"Роль з ID {id} не знайдено.");
            
            roleDb.Title = request.Title;
            var updatedRole = await _roleService.UpdateRoleAsync(roleDb, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<RoleDto>(updatedRole));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні Ролі з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Ролі з ID {id}: {ex.Message}");
        }
    }
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> SoftDeleteRole(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (removedById <= 0)
                return Unauthorized(new { message = "Користувач не авторизований." });
            
            var roleDb = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (roleDb == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            
            var removedRole = await _roleService.SoftDeleteRoleAsync(roleDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<RoleDto>(removedRole));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Ролі з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Ролі з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> RestoreRemovedRole(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (restoredById <= 0)
                return Unauthorized(new { message = "Користувач не авторизований." });
            
            var roleDb = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (roleDb == null)
                return NotFound($"Роль з ID {id} не знайдено.");
            

            var restoredRole = await _roleService.RestoreRemovedRoleAsync(roleDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<RoleDto>(restoredRole));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Ролі з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Ролі з ID {id}: {ex.Message}");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _roleService.DeleteRoleAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Ролі з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Ролі з ID {id}: {ex.Message}");
        }
    }
}

