using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;
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

    public RoleController(IRoleService roleService, IMapper mapper)
    {
        _roleService = roleService;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<ActionResult<Role>> PostRole(RoleCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var newRole = _mapper.Map<Role>(request);
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var createdRole = await _roleService.AddRoleAsync(newRole, createdById);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, _mapper.Map<RoleDTO>(createdRole));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
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
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        try
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            return Ok(_mapper.Map<RoleDTO>(role));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    [HttpGet("search")]
    public ActionResult<IEnumerable<Role>> SearchRoles(string title)
    {
        try
        {
            var roles = _roleService.SearchRoles(title);
            return Ok(_mapper.Map<IEnumerable<RoleDTO>>(roles));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    } 
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRole(int id, RoleCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var roleDb = await _roleService.GetRoleByIdAsync(id);
            if (roleDb == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            
            roleDb.Title = request.Title;
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedRole = await _roleService.UpdateRoleAsync(roleDb, modifiedById);
            
            return Ok(_mapper.Map<RoleDTO>(updatedRole));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> SoftDeleteRole(int id)
    {
        try
        {
            var roleDb = await _roleService.GetRoleByIdAsync(id);
            if (roleDb == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedRole = await _roleService.SoftDeleteRoleAsync(roleDb, removedById);
            
            return Ok(_mapper.Map<RoleDTO>(removedRole));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> RestoreRemovedRole(int id)
    {
        try
        {
            var roleDb = await _roleService.GetRoleByIdAsync(id);
            if (roleDb == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredRole = await _roleService.RestoreRemovedRoleAsync(roleDb, restoredById);
            
            return Ok(_mapper.Map<RoleDTO>(restoredRole));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var success = await _roleService.DeleteRoleAsync(id);
            if (!success)
            {
                return NotFound($"Роль з ID {{id}} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
}

