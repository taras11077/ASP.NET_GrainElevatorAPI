using AutoMapper;
using GrainElevatorAPI.Auth;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
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
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IConfiguration configuration, IRoleService roleService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _configuration = configuration;
        _roleService = roleService;
        _logger = logger;
    }
    
// реєстрація користувача
    [HttpPost("register")]
    public async Task<IActionResult> Register(EmployeeRegisterRequest request)
    {
        return await HandleRequestAsync(request, async () =>
        {
            var employeeDb = await _authService.Register(request.Email, request.Password, request.RoleId);
            
            var role = await _roleService.GetRoleByIdAsync(employeeDb.RoleId);
            if (role == null)
            {
                return BadRequest("Роль не знайдено.");
            }
            
            
            var tokenKey = _configuration.GetValue<string>("TokenKey")!;
            var expiryDate = DateTime.UtcNow.AddSeconds(_configuration.GetValue<int>("SessionTimeout"));
            
            var jwt = JwtGenerator.GenerateJwt(employeeDb, role.Title, tokenKey, expiryDate);

            return Created("token", jwt);
        });
    }

// логування користувача
    [HttpPost("login")]
    public async Task<IActionResult> Login(EmployeeLoginRequest request)
    {
        return await HandleRequestAsync(request, async () =>
        {
            var employee = await _authService.Login(request.Email, request.Password);
            
            if (employee == null)
            {
                return Unauthorized("Неправільний нікнейм або пароль.");
            }
            
            var role = await _roleService.GetRoleByIdAsync(employee.RoleId);
            if (role == null)
            {
                return BadRequest("Роль не знайдено.");
            }
            
            var tokenKey = _configuration.GetValue<string>("TokenKey")!;
            var expiryDate = DateTime.UtcNow.AddSeconds(_configuration.GetValue<int>("SessionTimeout"));
            
            var jwt = JwtGenerator.GenerateJwt(employee, role.Title, tokenKey, expiryDate);
            
            HttpContext.Session.SetInt32("EmployeeId", employee.Id);

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
            _logger.LogError($"Не переданий никнейм або пароль: {ex.Message}");
            return BadRequest(new { message = ex.Message }); // 400 Bad Request - якщо не переданий никнейм або пароль
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Співробітник з таким нікнеймом вже існує: {ex.Message}");
            return Conflict(new { message = ex.Message }); // 409 Conflict - якщо вже існує юзер з таким нікнеймом
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError($"Неправільні никнейм або пароль: {ex.Message}");
            return Unauthorized(new { message = ex.Message }); // 401 Unauthorized - якщо неправільні никнейм або пароль
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при валідації співробітника: {ex.Message}");
            return StatusCode(500, new { message = "Внутрішня помилка сервера при валідації співробітника." }); // 500 Internal Server Error - для всіх інших виключень
        }
    }
    
}
