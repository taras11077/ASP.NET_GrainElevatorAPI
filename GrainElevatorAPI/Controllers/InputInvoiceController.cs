using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Extensions;
using GrainElevatorAPI.Requests;
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
    

    [HttpPost]
    //[Authorize(Roles = "Admin, laboratory")]
    public async Task<ActionResult<InputInvoice>> CreateInputInvoice(InputInvoiceCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var newInputInvoice = _mapper.Map<InputInvoice>(request);
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var createdInputInvoice = await _inputInvoiceService.AddInputInvoiceAsync(newInputInvoice, createdById);
            _logger.LogInformation($"Прибуткову накладну з ID = {createdInputInvoice.Id} створено.");
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
    //[Authorize(Roles = "admin, laboratory")]
    public ActionResult<IEnumerable<InputInvoice>> GetInputInvoice([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var inputInvoices = _inputInvoiceService.GetInputInvoices(page, size);
            return Ok(_mapper.Map<IEnumerable<InputInvoiceDto>>(inputInvoices));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Прибуткових накладних: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Прибуткових накладних: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<ActionResult<InputInvoice>> GetInputInvoice(int id)
    {
        try
        {
            var inputInvoice = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);
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
    //[Authorize(Roles = "admin, laboratory")]
    public ActionResult<IEnumerable<InputInvoiceDto>> SearchInputInvoices(
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
            // передаємо параметри у сервіс для фільтрації
            var filteredInvoices = _inputInvoiceService.SearchInputInvoices(
                id, invoiceNumber, arrivalDate, vehicleNumber, physicalWeight, supplierId, productId, createdById, removedAt, page, size);

            return Ok(_mapper.Map<IEnumerable<InputInvoiceDto>>(filteredInvoices));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Прибуткової накладної за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Прибуткової накладної: {ex.Message}");
        }
    }

    
    [HttpPut("{id}")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> UpdateInputInvoice(int id, InputInvoiceUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);
            if (inputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }
            
            inputInvoiceDb.UpdateFromRequest(request);
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedInputInvoice = await _inputInvoiceService.UpdateInputInvoiceAsync(inputInvoiceDb, modifiedById);

            return Ok(_mapper.Map<InputInvoiceDto>(updatedInputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні Прибуткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Прибуткової накладної: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> SoftDeleteInputInvoice(int id)
    {
        try
        {
            var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);
            if (inputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }

            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedInputInvoice = await _inputInvoiceService.SoftDeleteInputInvoiceAsync(inputInvoiceDb, removedById);
            
            return Ok(_mapper.Map<InputInvoiceDto>(removedInputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Прибуткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Прибуткової накладної: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> RestoreRemovedInputInvoice(int id)
    {
        try
        {
            var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);
            if (inputInvoiceDb == null)
            {
                return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredInputInvoice = await _inputInvoiceService.RestoreRemovedInputInvoiceAsync(inputInvoiceDb, restoredById);

            return Ok(_mapper.Map<InputInvoiceDto>(restoredInputInvoice));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Прибуткової накладної з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Прибуткової накладної з ID {id}: {ex.Message}");
        }
    }
    
    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteInputInvoice(int id)
    {
        try
        {
            var success = await _inputInvoiceService.DeleteInputInvoiceAsync(id);
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


