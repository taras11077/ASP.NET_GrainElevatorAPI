using AutoMapper;
using GrainElevatorAPI.Auth;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IRoleService _roleService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, IConfiguration configuration, IRoleService roleService)
    {
        _authService = authService;
        _configuration = configuration;
        _roleService = roleService;
    }
    
// реєстрація користувача
    [HttpPost("register")]
    public async Task<IActionResult> Register(EmployeeRegisterRequest request)
    {
        return await HandleRequestAsync(request, async () =>
        {
            var userDb = await _authService.Register(request.Email, request.Password, request.RoleId);
            
            var role = await _roleService.GetRoleById(userDb.RoleId);
            if (role == null)
            {
                return BadRequest("Роль не знайдено.");
            }
            
            var tokenKey = _configuration.GetValue<string>("TokenKey")!;
            var expiryDate = DateTime.UtcNow.AddSeconds(_configuration.GetValue<int>("SessionTimeout"));
            
            var jwt = JwtGenerator.GenerateJwt(userDb, role.Title, tokenKey, expiryDate);
        
            HttpContext.Session.SetInt32("id", userDb.Id);

            return Created("token", jwt);
        });
    }

// логування користувача
    [HttpPost("login")]
    public async Task<IActionResult> Login(EmployeeLoginRequest request)
    {
        return await HandleRequestAsync(request, async () =>
        {
            var user = await _authService.Login(request.Email, request.Password);
            
            if (user == null)
            {
                return Unauthorized("Неправільний нікнейм або пароль.");
            }
            
            var role = await _roleService.GetRoleById(user.RoleId);
            if (role == null)
            {
                return BadRequest("Роль не знайдено.");
            }
            
            var tokenKey = _configuration.GetValue<string>("TokenKey")!;
            var expiryDate = DateTime.UtcNow.AddSeconds(_configuration.GetValue<int>("SessionTimeout"));
            
            var jwt = JwtGenerator.GenerateJwt(user, role.Title, tokenKey, expiryDate);

            return Created("token", jwt);
        });
    }
    
    
    // метод для валідації моделі та обробки помилок
    private async Task<IActionResult> HandleRequestAsync(EmployeeLoginRequest request, Func<Task<IActionResult>> action)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        try
        {
            return await action();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message }); // 400 Bad Request - якщо не переданий никнейм або пароль
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); // 409 Conflict - якщо вже існує юзер з таким нікнеймом
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message }); // 401 Unauthorized - якщо неправільні никнейм або пароль
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Виникла непередбачувана помилка." }); // 500 Internal Server Error - для всіх інших виключень
        }
    }
    
}
