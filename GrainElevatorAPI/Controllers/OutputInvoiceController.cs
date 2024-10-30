using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
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
    //[Authorize(Roles = "Admin, laboratory")]
    public async Task<ActionResult<OutputInvoice>> CreateOutputInvoice(OutputInvoiceCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var createdOutputInvoice = await _outputInvoiceService.CreateOutputInvoiceAsync(
                request.InvoiceNumber,
                request.SupplierTitle,
                request.ProductTitle,
                createdById, 
                cancellationToken);
            
            _logger.LogInformation($"Створено Видаткову накладну з ID = {createdOutputInvoice.Id}.");
            
            return CreatedAtAction(nameof(GetOutputInvoice), new { id = createdOutputInvoice.Id },
                _mapper.Map<OutputInvoiceDto>(createdOutputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Видаткової накладної: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Видаткової накладної: {ex.Message}");
        }
    }


    [HttpGet]
    //[Authorize(Roles = "admin, laboratory")]
    public ActionResult<IEnumerable<OutputInvoice>> GetOutputInvoices([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var outputInvoices = _outputInvoiceService.GetOutputInvoices(page, size, cancellationToken);
            return Ok(_mapper.Map<IEnumerable<OutputInvoiceDto>>(outputInvoices));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Прибуткових накладних: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Прибуткових накладних: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<ActionResult<OutputInvoice>> GetOutputInvoice(int id)
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
            _logger.LogError($"Внутрішня помилка сервера при отриманні Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Видаткової накладної з ID {id}: {ex.Message}");
        }
    }

    [HttpGet("search")]
    //[Authorize(Roles = "admin, laboratory")]
    public ActionResult<IEnumerable<OutputInvoiceDto>> SearchOutputInvoices(
        [FromQuery] int? id = null,
        [FromQuery] string? invoiceNumber = null,
        [FromQuery] DateTime? arrivalDate = null,
        [FromQuery] string? vehicleNumber = null,
        [FromQuery] int? physicalWeight = null,
        [FromQuery] int? supplierId = null,
        [FromQuery] int? productId = null,
        [FromQuery] int? createdById = null,
        [FromQuery] DateTime? removedAt = null,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            // передаємо параметри у сервіс для фільтрації
            var filteredInvoices = _outputInvoiceService.SearchOutputInvoices(
                id, invoiceNumber, arrivalDate, vehicleNumber, physicalWeight, supplierId, productId, createdById, removedAt, page, size, cancellationToken);

            return Ok(_mapper.Map<IEnumerable<OutputInvoiceDto>>(filteredInvoices));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Видаткової накладної за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Видаткової накладної: {ex.Message}");
        }
    }

    
    [HttpPut("{id}")]
    //[Authorize(Roles = "admin, laboratory")]
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
            var updatedOutputInvoice = await _outputInvoiceService.UpdateOutputInvoiceAsync(OutputInvoiceDb, modifiedById, cancellationToken);

            return Ok(_mapper.Map<OutputInvoiceDto>(updatedOutputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Видаткової накладної: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
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
            var removedOutputInvoice = await _outputInvoiceService.SoftDeleteOutputInvoiceAsync(OutputInvoiceDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<OutputInvoiceDto>(removedOutputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Видаткової накладної: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
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
            var restoredOutputInvoice = await _outputInvoiceService.RestoreRemovedOutputInvoiceAsync(OutputInvoiceDb, restoredById, cancellationToken);

            return Ok(_mapper.Map<OutputInvoiceDto>(restoredOutputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Видаткової накладної з ID {id}: {ex.Message}");
        }
    }
    
    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "admin")]
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
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Видаткової накладної з ID {id}: {ex.Message}");
        }
    }
    
}
