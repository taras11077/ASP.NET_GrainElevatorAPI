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
    public async Task<ActionResult<Role>> CreateRole(RoleCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var newRole = _mapper.Map<Role>(request);
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
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
    public ActionResult<IEnumerable<Role>> GetRoles([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var roles = _roleService.GetRoles(page, size);
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Ролей: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Ролей: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRoleById(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var role = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (role == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            return Ok(_mapper.Map<RoleDto>(role));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Ролі з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Ролі з ID {id}: {ex.Message}");
        }
    }
    
    [HttpGet("search")]
    public ActionResult<IEnumerable<Role>> SearchRoles(string title)
    {
        try
        {
            var roles = _roleService.SearchRoles(title);
            return Ok(_mapper.Map<IEnumerable<RoleDto>>(roles));
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
            var roleDb = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (roleDb == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            
            roleDb.Title = request.Title;
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
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
            var roleDb = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (roleDb == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
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
            var roleDb = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (roleDb == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
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

