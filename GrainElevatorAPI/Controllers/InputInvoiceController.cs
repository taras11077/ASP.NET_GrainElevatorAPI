using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.DTOs.Requests;
using GrainElevatorAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[Route("api/input-invoice")]
[ApiController]
public class InputInvoiceController : ControllerBase
{
    private readonly IInputInvoiceService _inputInvoiceService;
    private readonly IMapper _mapper;
    private readonly ILogger<InputInvoiceController> _logger;
    

    public InputInvoiceController(IInputInvoiceService inputInvoiceService, IMapper mapper, ILogger<InputInvoiceController> logger)
    {
        _inputInvoiceService = inputInvoiceService;
        _mapper = mapper;
        _logger = logger;
    }
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Laboratory")]
    public async Task<ActionResult<InputInvoiceDto>> CreateInputInvoice(InputInvoiceCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new { message = string.Join("; ", errors) });
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (createdById <= 0)
                return Unauthorized(new { message = "Користувач не авторизований." });
            
            
            var createdInputInvoice = await _inputInvoiceService.CreateInputInvoiceAsync(
                request.InvoiceNumber,
                request.ArrivalDate,
                request.SupplierTitle,
                request.ProductTitle,
                request.PhysicalWeight,
                request.VehicleNumber,
                createdById, 
                cancellationToken);
            
            _logger.LogInformation($"Створено прибуткову накладну з ID = {createdInputInvoice.Id}.");
            
            return CreatedAtAction(nameof(GetInputInvoice), new { id = createdInputInvoice.Id },
                _mapper.Map<InputInvoiceDto>(createdInputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"Внутрішня помилка сервера під час створення Прибуткової накладної.");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час створення Прибуткової накладної. Спробуйте пізніше." });
        }
    }


    [HttpGet]
    [Authorize(Roles = "Admin,Laboratory")]
    public async Task<ActionResult<IEnumerable<InputInvoiceDto>>> GetInputInvoices([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var (inputInvoices, totalCount) = await _inputInvoiceService.GetInputInvoicesAsync(page, size, cancellationToken);
            
            var inputInvoiceDtos = _mapper.Map<IEnumerable<InputInvoiceDto>>(inputInvoices);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            
            return Ok(inputInvoiceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Внутрішня помилка сервера під час отримання всіх Прибуткових накладних.");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час отримання Прибуткових накладних. Спробуйте пізніше."});
        }
    }
    
    
    [HttpGet("statistic")]
    [Authorize(Roles = "Admin,CEO")]
    public async Task<IActionResult> GetStatistics()
    {
        try
        {
            var cancellationToken = GetCancellationToken();
        
            // Отримання даних через сервіс
            var result = await _inputInvoiceService.GetTotalPhysicalWeightBySupplierAndProductAsync(cancellationToken);

            // Перевірка результату
            if (result.BySupplier == null || result.ByProduct == null)
            {
                _logger.LogWarning("Сервіс повернув некоректні дані для статистики.");
                return NotFound("Дані статистики не знайдені.");
            }

            // Повернення успішної відповіді з даними
            return Ok(new
            {
                BySupplier = result.BySupplier,
                ByProduct = result.ByProduct
            });
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Запит було скасовано.");
            return StatusCode(499, "Запит було скасовано.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Внутрішня помилка сервера під час отримання статистичних даних");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час отримання статистичних даних. Спробуйте пізніше."});
        }
    }

    [HttpGet("timeline-statistic")]
    [Authorize(Roles = "Admin,CEO")]
    public async Task<IActionResult> GetTimelineStatistics()
    {
        try
        {
            var cancellationToken = GetCancellationToken();

            // Отримання даних через сервіс
            var result = await _inputInvoiceService.GetTimelineStatisticsAsync(cancellationToken);

            if (result.BySupplierTimeline == null || result.ByProductTimeline == null)
            {
                _logger.LogWarning("Сервіс повернув некоректні дані для статистики по часі.");
                return NotFound("Дані статистики по часі не знайдені.");
            }

            // Повернення успішної відповіді з даними
            return Ok(new
            {
                BySupplierTimeline = result.BySupplierTimeline,
                ByProductTimeline = result.ByProductTimeline
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Внутрішня помилка сервера під час отримання статистичних даних по часу.");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час отримання статистичних даних по часу. Спробуйте пізніше."});
        }
    }

    


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Laboratory")]
    public async Task<ActionResult<InputInvoiceDto>> GetInputInvoice(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var inputInvoice = await _inputInvoiceService.GetInputInvoiceByIdAsync(id, cancellationToken);
            if (inputInvoice == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<InputInvoiceDto>(inputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"Внутрішня помилка сервера під час отримання Прибуткової накладної з ID {id}.");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час отримання Прибуткової накладної. Спробуйте пізніше."});
        }
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Laboratory,CEO")]
    public async Task<ActionResult<IEnumerable<InputInvoiceDto>>> SearchInputInvoices(
        [FromQuery] int? id = null,
        [FromQuery] string? invoiceNumber = null,
        [FromQuery] DateTime? arrivalDate = null,
        [FromQuery] string? vehicleNumber = null,
        [FromQuery] int? physicalWeight = null,
        [FromQuery] string? supplierTitle = null,
        [FromQuery] string? productTitle = null,
        [FromQuery] string? createdByName = null,
        [FromQuery] DateTime? removedAt = null,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? sortField = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var cancellationToken = GetCancellationToken();

            var (filteredInvoices, totalCount) = await _inputInvoiceService.SearchInputInvoices(
                id, 
                invoiceNumber,
                arrivalDate, 
                vehicleNumber, 
                physicalWeight, 
                supplierTitle, 
                productTitle, 
                createdByName, 
                removedAt, 
                page, 
                size, 
                sortField, sortOrder,
                cancellationToken);

            
            var inputInvoiceDtos = _mapper.Map<IEnumerable<InputInvoiceDto>>(filteredInvoices);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            return Ok(inputInvoiceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Внутрішня помилка сервера при отримання Прибуткової накладної за параметрами.");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час отримання Прибуткової накладної за параметрами. Спробуйте пізніше."});
        }
    }

    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Laboratory")]
    public async Task<ActionResult<InputInvoiceDto>> UpdateInputInvoice(int id, InputInvoiceUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id, cancellationToken);
            if (inputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }
            
            inputInvoiceDb.UpdateFromRequest(request);
            
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (modifiedById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            _logger.LogInformation($"Retrieved EmployeeId {modifiedById} from session.");
            var updatedInputInvoice = await _inputInvoiceService.UpdateInputInvoiceAsync(inputInvoiceDb, modifiedById, cancellationToken);

            
            return Ok(_mapper.Map<InputInvoiceDto>(updatedInputInvoice));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Unauthorized access while updating invoice.");
            return Unauthorized(new { message = "Ви не маєте прав для цієї операції." }); // 401 Unauthorized
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"Invalid operation while updating invoice with ID {id}");
            return BadRequest(new { message = "Некоректна операція. Перевірте введені дані." }); // 400 Bad Request
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Внутрішня помилка сервера під час оновлення Прибуткової накладної з ID {id}");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час оновлення Прибуткової накладної. Спробуйте пізніше." }); // 500 Internal Server Error
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    [Authorize(Roles = "Admin,Laboratory")]
    public async Task<ActionResult<InputInvoiceDto>> SoftDeleteInputInvoice(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id, cancellationToken);
            if (inputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }

            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (removedById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }

            var removedInputInvoice = await _inputInvoiceService.SoftDeleteInputInvoiceAsync(inputInvoiceDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<InputInvoiceDto>(removedInputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Внутрішня помилка сервера під час soft-видалення Прибуткової накладної з ID {id}.");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час soft-видалення Прибуткової накладної. Спробуйте пізніше." });
        }
    }
    

    [HttpPatch("{id}/restore")]
    [Authorize(Roles = "Admin,Laboratory")]
    public async Task<ActionResult<InputInvoiceDto>> RestoreRemovedInputInvoice(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id, cancellationToken);
            if (inputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (restoredById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var restoredInputInvoice = await _inputInvoiceService.RestoreRemovedInputInvoiceAsync(inputInvoiceDb, restoredById, cancellationToken);

            return Ok(_mapper.Map<InputInvoiceDto>(restoredInputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Внутрішня помилка сервера під час відновлення Прибуткової накладної з ID {id}.");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час відновлення Прибуткової накладної. Спробуйте пізніше."});
        }
    }
    
    [HttpDelete("{id}/hard-remove")]
    [Authorize(Roles = "Admin,Laboratory")]
    public async Task<IActionResult> DeleteInputInvoice(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _inputInvoiceService.DeleteInputInvoiceAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"Внутрішня помилка сервера під час hard-видалення Прибуткової накладної з ID {id}.");
            return StatusCode(500, new { message = "Внутрішня помилка сервера під час hard-видалення Прибуткової накладної. Спробуйте пізніше."});
        }
    }
    
}


