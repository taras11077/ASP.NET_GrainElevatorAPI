using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GrainElevatorAPI.Requests;

namespace GrainElevatorAPI.Controllers;

[Route("api/role")]
[ApiController]
public class RoleController : ControllerBase
{
   private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }


    // POST: api/Role
    [HttpPost]
    public async Task<ActionResult<Role>> AddRole(RoleCreateRequest request)
    {
        try
        {
            var createdRole = await _roleService.AddRole(request.Title);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // GET: api/Role
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

    // GET: api/Role/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        try
        {
            var employee = await _roleService.GetRoleById(id);
            if (employee == null)
            {
                return NotFound($"Роль з ID {id} не знайдено.");
            }
            return Ok(employee);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // DELETE: api/Role/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var success = await _roleService.DeleteRole(id);
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

    // GET: api/Role/search?name=John
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

