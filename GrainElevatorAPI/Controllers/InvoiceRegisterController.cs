using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;
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
    public async Task<ActionResult<Register>> CreateRegister([FromBody] InvoiceRegisterCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        try
        {
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();

            var cancellationToken = GetCancellationToken();
        
            // створення Реєстру (доробка продукції)
            var createdRegister = await _invoiceRegisterService.CreateRegisterAsync(
                request.SupplierId,
                request.ProductId,
                request.ArrivalDate,
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
    public ActionResult<IEnumerable<Register>> GetRegisters([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var registers = _invoiceRegisterService.GetRegisters(page, size);
            return Ok(_mapper.Map<IEnumerable<InvoiceRegisterDto>>(registers));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Реєстрів: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Реєстрів: {ex.Message}");
        }
    }
    
    
    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<ActionResult<Register>> GetRegisterById(int id)
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
    
    
    
    

