using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
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

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
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
            var createdRole = await _roleService.AddRoleAsync(request.Title);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
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
            return Ok(role);
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


    [HttpGet("search")]
    public ActionResult<IEnumerable<Role>> SearchRoles(string title)
    {
        try
        {
            var roles = _roleService.SearchRoles(title);
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    } 
    
    
}

