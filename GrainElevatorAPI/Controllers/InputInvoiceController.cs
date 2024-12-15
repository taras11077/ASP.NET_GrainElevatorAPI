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
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
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
            _logger.LogError($"Внутрішня помилка сервера при створенні Прибуткової накладної: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Прибуткової накладної: {ex.Message}");
        }
    }


    [HttpGet]
    [Authorize(Roles = "Admin,Laboratory")]
    public async Task<ActionResult<IEnumerable<InputInvoiceDto>>> GetInputInvoices([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var (inputInvoices, totalCount) = await _inputInvoiceService.GetInputInvoices(page, size, cancellationToken);
            
            var inputInvoiceDtos = _mapper.Map<IEnumerable<InputInvoiceDto>>(inputInvoices);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            
            return Ok(inputInvoiceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Прибуткових накладних: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Прибуткових накладних: {ex.Message}");
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
            _logger.LogError($"Внутрішня помилка сервера при отриманні Прибуткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Прибуткової накладної з ID {id}: {ex.Message}");
        }
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Laboratory")]
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
            _logger.LogError($"Внутрішня помилка сервера при отриманні Прибуткової накладної за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Прибуткової накладної: {ex.Message}");
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
            if(modifiedById == 0)
                throw new UnauthorizedAccessException("Користувач не авторизований."); 
            
            _logger.LogInformation($"Retrieved EmployeeId {modifiedById} from session.");
            var updatedInputInvoice = await _inputInvoiceService.UpdateInputInvoiceAsync(inputInvoiceDb, modifiedById, cancellationToken);

            
            return Ok(_mapper.Map<InputInvoiceDto>(updatedInputInvoice));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex.Message);
            return Unauthorized(new { message = ex.Message });// 401 Unauthorized
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex.Message );
            return BadRequest(new { message = ex.Message }); // 400 Bad Request
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні Прибуткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Прибуткової накладної: {ex.Message}");// 500 Internal Server Error
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
            removedById = 1; // TODO
            var removedInputInvoice = await _inputInvoiceService.SoftDeleteInputInvoiceAsync(inputInvoiceDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<InputInvoiceDto>(removedInputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Прибуткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Прибуткової накладної: {ex.Message}");
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
            var restoredInputInvoice = await _inputInvoiceService.RestoreRemovedInputInvoiceAsync(inputInvoiceDb, restoredById, cancellationToken);

            return Ok(_mapper.Map<InputInvoiceDto>(restoredInputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Прибуткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Прибуткової накладної з ID {id}: {ex.Message}");
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
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Прибуткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Прибуткової накладної з ID {id}: {ex.Message}");
        }
    }
    
}


