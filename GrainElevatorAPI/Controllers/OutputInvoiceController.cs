using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.Extensions;
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
    public async Task<ActionResult<OutputInvoiceDto>> CreateOutputInvoice(OutputInvoiceCreateRequest request)
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
                request.VehicleNumber,
                request.SupplierId,
                request.ProductId,
                request.ProductCategory,
                request.ProductWeight,
                createdById, 
                cancellationToken);
            
            _logger.LogInformation($"Створено Видаткову накладну з ID = {createdOutputInvoice.Id}.");
            
            return CreatedAtAction(nameof(GetOutputInvoice), new { id = createdOutputInvoice.Id },
                _mapper.Map<OutputInvoiceDto>(createdOutputInvoice));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Помилка валідації під час створення Видаткової накладної: {ex.Message}");
            return BadRequest($"Помилка валідації: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Некоректна операція під час створення Видаткової накладної: {ex.Message}");
            return StatusCode(409, $"Конфлікт даних: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час створення Видаткової накладної: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpGet]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<ActionResult> GetOutputInvoices([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            // _logger.LogInformation("Початок отримання видаткових накладних із сервісу.");
            // var serviceStart = DateTime.UtcNow;
            //
            // var outputInvoices = await _outputInvoiceService.GetOutputInvoices(page, size, cancellationToken);
            //
            // var serviceEnd = DateTime.UtcNow;
            // _logger.LogInformation("Сервіс завершив запит за {Duration} мс", (serviceEnd - serviceStart).TotalMilliseconds);
            //
            // _logger.LogInformation("Початок маппінгу видаткових накладних.");
            // var mappingStart = DateTime.UtcNow;
            //
            // var outputInvoiceDtos = _mapper.Map<IEnumerable<OutputInvoiceDto>>(outputInvoices);
            //
            // var mappingEnd = DateTime.UtcNow;
            // _logger.LogInformation("Маппінг завершено за {Duration} мс", (mappingEnd - mappingStart).TotalMilliseconds);
            
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
    //[Authorize(Roles = "admin, laboratory")]
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
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<ActionResult> SearchOutputInvoices(
        [FromQuery] int? id = null,
        [FromQuery] string? invoiceNumber = null,
        [FromQuery] DateTime? shipmentDate = null,
        [FromQuery] string? vehicleNumber = null,
        [FromQuery] int? supplierId = null,
        [FromQuery] int? productId = null,
        [FromQuery] string? productCategory = null,
        [FromQuery] int? productWeight = null,
        [FromQuery] int? createdById = null,
        [FromQuery] DateTime? removedAt = null,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            // передаємо параметри у сервіс для фільтрації
            var filteredInvoices = await _outputInvoiceService.SearchOutputInvoices(
                id, invoiceNumber, shipmentDate, vehicleNumber,  supplierId, productId, productCategory,  productWeight, createdById, removedAt, page, size, cancellationToken);

            var outputInvoiceDtos = _mapper.Map<IEnumerable<OutputInvoiceDto>>(filteredInvoices);
            return Ok(outputInvoiceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час отримання Видаткової накладної за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час отримання Видаткової накладної: {ex.Message}");
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
            _logger.LogError($"Внутрішня помилка сервера під час hard-видалення Видаткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час hard-видалення Видаткової накладної з ID {id}: {ex.Message}");
        }
    }
    
}
