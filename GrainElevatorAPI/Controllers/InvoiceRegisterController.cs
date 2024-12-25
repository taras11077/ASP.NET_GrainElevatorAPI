using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.DTOs.Requests;
using GrainElevatorAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace GrainElevatorAPI.Controllers;

[Route("api/register")]
[ApiController]
public class InvoiceRegisterController : ControllerBase
{
    private readonly IInvoiceRegisterService _invoiceRegisterService;
    private readonly IMapper _mapper;
    private readonly ILogger<InvoiceRegisterController> _logger;

    public InvoiceRegisterController(IInvoiceRegisterService invoiceRegisterService, IMapper mapper, ILogger<InvoiceRegisterController> logger)
    {
        _invoiceRegisterService = invoiceRegisterService;
        _mapper = mapper;
        _logger = logger;
    }
    
    // приватний метод для отримання токена скасування (спрацьовує якщо клієнт скасує запит)
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    
    [HttpPost]
    [Authorize(Roles = "Admin,Technologist")]
    public async Task<ActionResult<InvoiceRegisterDto>> CreateRegister([FromBody] InvoiceRegisterCreateRequest request)
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
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            // створення Реєстру (доробка продукції)
            var createdRegister = await _invoiceRegisterService.CreateInvoiceRegisterAsync(
                request.RegisterNumber,
                request.ArrivalDate,
                request.SupplierTitle,
                request.ProductTitle,
                request.WeedImpurityBase,
                request.MoistureBase,
                createdById,
                cancellationToken);
            
            return CreatedAtAction(nameof(GetRegisters), new { id = createdRegister.Id },
                _mapper.Map<InvoiceRegisterDto>(createdRegister));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Помилка створення Реєстру: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Помилка авторизації: {ex.Message}");
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера: {ex.Message}");
            return StatusCode(500, new { message = "Внутрішня помилка сервера. Спробуйте пізніше.", detail = ex.Message });
        }
    }
    
    
    [HttpGet]
    [Authorize(Roles = "Admin,Technologist")]
    public async Task<ActionResult<IEnumerable<InvoiceRegisterDto>>> GetRegisters([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var registers = await _invoiceRegisterService.GetInvoiceRegistersAsync(page, size, cancellationToken);

            var registerDtos = _mapper.Map<IEnumerable<InvoiceRegisterDto>>(registers);
            return Ok(registerDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Реєстрів: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Реєстрів: {ex.Message}");
        }
    }
    
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Technologist")]
    public async Task<ActionResult<InvoiceRegisterDto>> GetRegisterById(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var register = await _invoiceRegisterService.GetInvoiceRegisterByIdAsync(id, cancellationToken);
            if (register == null)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<InvoiceRegisterDto>(register));
        }
        catch (Exception ex)
        {
            
            _logger.LogError($"Внутрішня помилка сервера під час отримання Реєстру з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час отримання Реєстру з ID {id}: {ex.Message}");
        }
    }
    
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Technologist")]
    public async Task<ActionResult<IEnumerable<InvoiceRegisterDto>>> SearchRegisters(
        [FromQuery] string? registerNumber,
        [FromQuery] DateTime? arrivalDate,
        [FromQuery] int? physicalWeightReg,
        [FromQuery] int? shrinkageReg,
        [FromQuery] int? wasteReg,
        [FromQuery] int? accWeightReg,
        [FromQuery] double? weedImpurityBase,
        [FromQuery] double? moistureBase,
        [FromQuery] string? supplierTitle,
        [FromQuery] string? productTitle,
        [FromQuery] string? createdByName,
        [FromQuery] DateTime? removedAt,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? sortField = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var cancellationToken = GetCancellationToken();

            var (filteredRegisters, totalCount) = await _invoiceRegisterService.SearchInvoiceRegistersAsync(
                registerNumber,
                arrivalDate,
                physicalWeightReg,
                shrinkageReg,
                wasteReg,
                accWeightReg,
                weedImpurityBase,
                moistureBase,
                supplierTitle,
                productTitle,
                createdByName,
                removedAt, 
                page, 
                size,
                sortField, sortOrder,
                cancellationToken);

            var registerDtos = _mapper.Map<IEnumerable<InvoiceRegisterDto>>(filteredRegisters);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            
            return Ok(registerDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Реєстру за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Реєстру за параметрами: {ex.Message}");
        }
    }
    
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Technologist")]
    public async Task<IActionResult> UpdateInvoiceRegister(int id, InvoiceRegisterUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var cancellationToken = GetCancellationToken();
            
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (modifiedById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var updatedRegister = await _invoiceRegisterService.UpdateInvoiceRegisterAsync(
                id, 
                request.RegisterNumber, 
                request.WeedImpurityBase, 
                request.MoistureBase, 
                modifiedById, 
                cancellationToken);

            return Ok(_mapper.Map<InvoiceRegisterDto>(updatedRegister));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час оновлення Реєстру з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час оновлення Реєстру з ID {id}: {ex.Message}");
        }
    }
    
    [HttpPatch("{id}/soft-remove")]
    [Authorize(Roles = "Admin,Technologist")]
    public async Task<IActionResult> SoftDeleteRegister(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var registerDb = await _invoiceRegisterService.GetInvoiceRegisterByIdAsync(id, cancellationToken);
            if (registerDb == null)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (removedById <= 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            
            var removedRegister = await _invoiceRegisterService.SoftDeleteInvoiceRegisterAsync(registerDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<InvoiceRegisterDto>(removedRegister));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час видалення Реєстру з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час видалення Реєстру з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RestoreRemovedRegister(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var registerDb = await _invoiceRegisterService.GetInvoiceRegisterByIdAsync(id, cancellationToken);
            if (registerDb == null)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (restoredById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var restoredRegister = await _invoiceRegisterService.RestoreRemovedInvoiceRegisterAsync(registerDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<InvoiceRegisterDto>(restoredRegister));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час відновлення Реєстру з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час відновлення Реєстру з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}/hard-remove")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRegister(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var success = await _invoiceRegisterService.DeleteInvoiceRegisterAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час hard-видалення Реєстру з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час hard-видалення Реєстру з ID {id}: {ex.Message}");
        }
    }
}
    
    
    
    

