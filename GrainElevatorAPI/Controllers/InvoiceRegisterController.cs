using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.DTOs.Requests;
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
    //[Authorize(Roles = "Admin, Technologist")]
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
            
            // створення Реєстру (доробка продукції)
            var createdRegister = await _invoiceRegisterService.CreateRegisterAsync(
                request.RegisterNumber,
                request.SupplierId,
                request.ProductId,
                request.WeedImpurityBase,
                request.MoistureBase,
                request.LaboratoryCardIds,
                createdById,
                cancellationToken);
            
            return CreatedAtAction(nameof(GetRegisters), new { id = createdRegister.Id },
                _mapper.Map<InvoiceRegisterDto>(createdRegister));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Реєстра: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Реєстра: {ex.Message}");
        }
    }
    
    
    [HttpGet]
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<ActionResult<IEnumerable<InvoiceRegisterDto>>> GetRegisters([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var registers = await _invoiceRegisterService.GetRegistersAsync(page, size, cancellationToken);

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
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<ActionResult<InvoiceRegisterDto>> GetRegisterById(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var register = await _invoiceRegisterService.GetRegisterByIdAsync(id, cancellationToken);
            if (register == null)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<InvoiceRegisterDto>(register));
        }
        catch (Exception ex)
        {
            
            _logger.LogError($"Внутрішня помилка сервера при отриманні Реєстра з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Реєстра з ID {id}: {ex.Message}");
        }
    }
    
    [HttpGet("search")]
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<ActionResult<IEnumerable<InvoiceRegisterDto>>> SearchRegisters(
        [FromQuery] int? id,
        [FromQuery] string? registerNumber,
        [FromQuery] DateTime? arrivalDate,
        [FromQuery] int? supplierId,
        [FromQuery] int? productId,
        [FromQuery] int? physicalWeightReg,
        [FromQuery] int? shrinkageReg,
        [FromQuery] int? wasteReg,
        [FromQuery] int? accWeightReg,
        [FromQuery] double? weedImpurityBase,
        [FromQuery] double? moistureBase,
        [FromQuery] int? createdById,
        [FromQuery] DateTime? removedAt,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            // передаємо параметри у сервіс для фільтрації
            var filteredRegisters = await _invoiceRegisterService.SearchRegistersAsync(
                id, 
                registerNumber,
                arrivalDate,
                supplierId,
                productId,
                physicalWeightReg,
                shrinkageReg,
                wasteReg,
                accWeightReg,
                weedImpurityBase,
                moistureBase,
                createdById, 
                removedAt, 
                page, 
                size,
                cancellationToken);

            var registerDtos = _mapper.Map<IEnumerable<InvoiceRegisterDto>>(filteredRegisters);
            return Ok(registerDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Реєстру за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Реєстру за параметрами: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> SoftDeleteRegister(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var registerDb = await _invoiceRegisterService.GetRegisterByIdAsync(id, cancellationToken);
            if (registerDb == null)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedRegister = await _invoiceRegisterService.SoftDeleteRegisterAsync(registerDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<InvoiceRegisterDto>(removedRegister));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Реєстру з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Реєстру з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> RestoreRemovedRegister(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var registerDb = await _invoiceRegisterService.GetRegisterByIdAsync(id, cancellationToken);
            if (registerDb == null)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredRegister = await _invoiceRegisterService.RestoreRemovedRegisterAsync(registerDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<InvoiceRegisterDto>(restoredRegister));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Реєстру з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Реєстру з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRegister(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var success = await _invoiceRegisterService.DeleteRegisterAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Реєстра з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Реєстра з ID {id}: {ex.Message}");
        }
    }
}
    
    
    
    

