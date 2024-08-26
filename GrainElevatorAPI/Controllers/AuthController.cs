using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }
    
// реєстрація користувача
    [HttpPost("register")]
    public async Task<IActionResult> Register(EmployeeRegisterRequest request)
    {
        return await HandleRequestAsync(request, async () =>
        {
            var userDb = await _authService.Register(request.Email, request.Password, request.RoleId);
            var jwt = JwtGenerator.GenerateJwt(userDb, _configuration.GetValue<string>("TokenKey")!, DateTime.UtcNow.AddMinutes(5));
        
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
            var jwt = JwtGenerator.GenerateJwt(user, _configuration.GetValue<string>("TokenKey")!, DateTime.UtcNow.AddMinutes(5));

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
