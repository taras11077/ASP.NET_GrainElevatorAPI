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

    public InputInvoiceController(IInputInvoiceService inputInvoiceService, IMapper mapper)
    {
        _inputInvoiceService = inputInvoiceService;
        _mapper = mapper;
    }
    

    [HttpPost]
    [Authorize(Roles = "admin, laboratory")]
    public async Task<ActionResult<InputInvoice>> PostInputInvoice(InputInvoiceCreateRequest request)
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
            return CreatedAtAction(nameof(GetInputInvoice), new { id = createdInputInvoice.Id },
                _mapper.Map<InputInvoiceDTO>(createdInputInvoice));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpGet]
    //[Authorize(Roles = "admin, laboratory")]
    public ActionResult<IEnumerable<InputInvoice>> GetInputInvoice([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var inputInvoices = _inputInvoiceService.GetInputInvoices(page, size);
            return Ok(_mapper.Map<IEnumerable<InputInvoiceDTO>>(inputInvoices));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
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

            return Ok(_mapper.Map<InputInvoiceDTO>(inputInvoice));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    [HttpGet("search")]
    //[Authorize(Roles = "admin, laboratory")]
    public ActionResult<IEnumerable<InputInvoiceDTO>> SearchInputInvoices(
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

            return Ok(_mapper.Map<IEnumerable<InputInvoiceDTO>>(filteredInvoices));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    
    [HttpPut("{id}")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> PutInputInvoice(int id, InputInvoiceUpdateRequest request)
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

            return Ok(_mapper.Map<InputInvoiceDTO>(updatedInputInvoice));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
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
            
            return Ok(_mapper.Map<InputInvoiceDTO>(removedInputInvoice));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
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

            return Ok(_mapper.Map<InputInvoiceDTO>(restoredInputInvoice));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
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
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
}


