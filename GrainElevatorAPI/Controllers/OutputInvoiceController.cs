using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[Route("api/output-invoice")]
[ApiController]
public class OutputInvoiceController: ControllerBase
{
    private readonly IOutputInvoiceService _outputInvoiceService;
    private readonly IMapper _mapper;
    private readonly ILogger<OutputInvoiceController> _logger;
    

    public OutputInvoiceController(IOutputInvoiceService outputInvoiceService, IMapper mapper, ILogger<OutputInvoiceController> logger)
    {
        _outputInvoiceService = outputInvoiceService;
        _mapper = mapper;
        _logger = logger;
    }
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    
    [HttpPost]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<OutputInvoiceDto>> CreateOutputInvoice(OutputInvoiceCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var cancellationToken = GetCancellationToken();
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (createdById <= 0)
                return Unauthorized(new { message = "Користувач не авторизований." });

            var createdOutputInvoice = await _outputInvoiceService.CreateOutputInvoiceAsync(
                request.InvoiceNumber,
                request.ShipmentDate,
                request.VehicleNumber,
                request.SupplierTitle,
                request.ProductTitle,
                request.ProductCategory,
                request.ProductWeight,
                createdById,
                cancellationToken);

            _logger.LogInformation($"Створено Видаткову накладну з ID = {createdOutputInvoice.Id}.");

            return CreatedAtAction(
                nameof(GetOutputInvoice),
                new { id = createdOutputInvoice.Id },
                _mapper.Map<OutputInvoiceDto>(createdOutputInvoice));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Помилка пошуку даних: {ex.Message}");
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Помилка операції: {ex.Message}");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера: {ex.Message}");
            return StatusCode(500, new { message = "Внутрішня помилка сервера. Спробуйте пізніше." });
        }
    }


    [HttpGet]
    [Authorize(Roles = "Admin,Accountant,CEO")]
    public async Task<ActionResult> GetOutputInvoices([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var outputInvoices = await _outputInvoiceService.GetOutputInvoices(page, size, cancellationToken);
            var outputInvoiceDtos = _mapper.Map<IEnumerable<OutputInvoiceDto>>(outputInvoices);
            
            return Ok(outputInvoiceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Прибуткових накладних: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Прибуткових накладних: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Accountant,CEO")]
    public async Task<ActionResult> GetOutputInvoice(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var OutputInvoice = await _outputInvoiceService.GetOutputInvoiceByIdAsync(id, cancellationToken);
            if (OutputInvoice == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<OutputInvoiceDto>(OutputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час отримання Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час отримання Видаткової накладної з ID {id}: {ex.Message}");
        }
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Accountant,CEO")]
    public async Task<ActionResult> SearchOutputInvoices(
        [FromQuery] string? invoiceNumber = null,
        [FromQuery] DateTime? shipmentDate = null,
        [FromQuery] string? vehicleNumber = null,
        [FromQuery] string? supplierTitle = null,
        [FromQuery] string? productTitle = null,
        [FromQuery] string? productCategory = null,
        [FromQuery] int? productWeight = null,
        [FromQuery] string? createdByName = null,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? sortField = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var cancellationToken = GetCancellationToken();

            var (filteredInvoices, totalCount) = await _outputInvoiceService.SearchOutputInvoices(
                invoiceNumber, 
                shipmentDate, 
                vehicleNumber,  
                supplierTitle, 
                productTitle, 
                productCategory,  
                productWeight, 
                createdByName, 
                page, size, 
                sortField, sortOrder,
                cancellationToken);

            var outputInvoiceDtos = _mapper.Map<IEnumerable<OutputInvoiceDto>>(filteredInvoices);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            return Ok(outputInvoiceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час отримання Видаткової накладної за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час отримання Видаткової накладної: {ex.Message}");
        }
    }

    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> UpdateOutputInvoice(int id, OutputInvoiceUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var OutputInvoiceDb = await _outputInvoiceService.GetOutputInvoiceByIdAsync(id, cancellationToken);
            if (OutputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }
            
            OutputInvoiceDb.UpdateFromRequest(request);
            
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (modifiedById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var updatedOutputInvoice = await _outputInvoiceService.UpdateOutputInvoiceAsync(OutputInvoiceDb, modifiedById, cancellationToken);

            var updatedOutputInvoiceDto = _mapper.Map<OutputInvoiceDto>(updatedOutputInvoice);
            
            return Ok(updatedOutputInvoiceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час оновлення Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час оновлення Видаткової накладної: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> SoftDeleteOutputInvoice(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var OutputInvoiceDb = await _outputInvoiceService.GetOutputInvoiceByIdAsync(id, cancellationToken);
            if (OutputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }

            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (removedById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var removedOutputInvoice = await _outputInvoiceService.SoftDeleteOutputInvoiceAsync(OutputInvoiceDb, removedById, cancellationToken);

            var removedOutputInvoiceDto = _mapper.Map<OutputInvoiceDto>(removedOutputInvoice);
            
            return Ok(removedOutputInvoiceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час видалення Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час видалення Видаткової накладної: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RestoreRemovedOutputInvoice(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var OutputInvoiceDb = await _outputInvoiceService.GetOutputInvoiceByIdAsync(id, cancellationToken);
            if (OutputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (restoredById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var restoredOutputInvoice = await _outputInvoiceService.RestoreRemovedOutputInvoiceAsync(OutputInvoiceDb, restoredById, cancellationToken);

            var restoredOutputInvoiceDto = _mapper.Map<OutputInvoiceDto>(restoredOutputInvoice);
            
            return Ok(restoredOutputInvoiceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час відновлення Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час відновлення Видаткової накладної з ID {id}: {ex.Message}");
        }
    }
    
    [HttpDelete("{id}/hard-remove")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteOutputInvoice(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _outputInvoiceService.DeleteOutputInvoiceAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час hard-видалення Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час hard-видалення Видаткової накладної з ID {id}: {ex.Message}");
        }
    }
    
}
