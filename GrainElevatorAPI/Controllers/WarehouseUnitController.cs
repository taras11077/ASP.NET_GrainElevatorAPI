using System.ComponentModel.Design;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[Route("api/warehouse-unit")]
[ApiController]
public class WarehouseUnitController : ControllerBase
{
    private readonly IWarehouseUnitService _warehouseUnitService;
    private readonly IMapper _mapper;
    private readonly ILogger<InvoiceRegisterController> _logger;

    public WarehouseUnitController(IWarehouseUnitService warehouseUnitService, IMapper mapper, ILogger<InvoiceRegisterController> logger)
    {
        _warehouseUnitService = warehouseUnitService;
        _mapper = mapper;
        _logger = logger;
    }
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    
        [HttpPost]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<WarehouseUnitDto>> CreateWarehouseUnit(WarehouseUnitCreateRequest request)
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
            
            var createdWarehouseUnit = await _warehouseUnitService.CreateWarehouseUnitAsync(
                request.SupplierTitle,
                request.ProductTitle,
                createdById, 
                cancellationToken);
            
            _logger.LogInformation($"Створено Складський юніт з ID = {createdWarehouseUnit.Id}.");
            
            return CreatedAtAction(nameof(GetWarehouseUnits), new { id = createdWarehouseUnit.Id },
                _mapper.Map<WarehouseUnitDto>(createdWarehouseUnit));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Помилка валідації під час створення Складського юніта: {ex.Message}");
            return BadRequest($"Помилка валідації: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Некоректна операція під час створення Складського юніта: {ex.Message}");
            return StatusCode(409, $"Конфлікт даних: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час створення Складського юніта: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    
    [HttpGet]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<IEnumerable<WarehouseUnitDto>>> GetWarehouseUnits([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var warehouseUnits = await _warehouseUnitService.GetPagedWarehouseUnitsAsync(page, size, cancellationToken);

            var warehouseUnitDtos = _mapper.Map<IEnumerable<WarehouseUnitDto>>(warehouseUnits);
            return Ok(warehouseUnitDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Складських юнітів: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Складських юнітів: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Technologist")]
    public async Task<ActionResult<WarehouseUnitDto>> GetWarehouseUnitById(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var warehouseUnit = await _warehouseUnitService.GetWarehouseUnitByIdAsync(id, cancellationToken);
            if (warehouseUnit == null)
            {
                return NotFound($"Складського юніта з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<WarehouseUnitDto>(warehouseUnit));
        }
        catch (Exception ex)
        {
            
            _logger.LogError($"Внутрішня помилка сервера під час отримання Складського юніта з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час отримання Складського юніта з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<IEnumerable<WarehouseUnitDto>>> SearchWarehouseUnits(
        [FromQuery] string? supplierTitle,
        [FromQuery] string? productTitle,
        [FromQuery] string? createdByName,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? sortField = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var cancellationToken = GetCancellationToken();

            var (filteredWarehouseUnits, totalCount) = await _warehouseUnitService.SearchWarehouseUnitsAsync(
                supplierTitle,
                productTitle,
                createdByName,
                page, 
                size,
                sortField, sortOrder,
                cancellationToken);

            var warehouseUnitDtos = _mapper.Map<IEnumerable<WarehouseUnitDto>>(filteredWarehouseUnits);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            
            return Ok(warehouseUnitDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Складського юніта за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Складського юніта за параметрами: {ex.Message}");
        }
    }
    
    [HttpPatch("{id}/soft-remove")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> SoftDeleteWarehouseUnit(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var warehouseUnitDb = await _warehouseUnitService.GetWarehouseUnitByIdAsync(id, cancellationToken);
            if (warehouseUnitDb == null)
            {
                return NotFound($"Складського юніта з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (removedById <= 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var removedWarehouseUnit = await _warehouseUnitService.SoftDeleteWarehouseUnitAsync(warehouseUnitDb, removedById, cancellationToken);
            var removedWarehouseUnitDto = _mapper.Map<WarehouseUnitDto>(removedWarehouseUnit);
            
            return Ok(new { message = $"Складський юніт з ID {id} успішно видалено.", removedWarehouseUnitDto });
        }
        catch (CheckoutException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час видалення Складського юніта з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час видалення Складського юніта з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RestoreRemovedRegister(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var warehouseUnitDb = await _warehouseUnitService.GetWarehouseUnitByIdAsync(id, cancellationToken);
            if (warehouseUnitDb == null)
            {
                return NotFound($"Складського юніта з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (restoredById == 0)
            {
                return Unauthorized(new { message = "Користувач не авторизований." });
            }
            
            var restoredWarehouseUnit = await _warehouseUnitService.RestoreRemovedWarehouseUnitAsync(warehouseUnitDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<InvoiceRegisterDto>(restoredWarehouseUnit));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час відновлення Складського юніта з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час відновлення Складського юніта з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}/hard-remove")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteWarehouseUnit(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var success = await _warehouseUnitService.DeleteWarehouseUnitAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Складського юніта з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера під час hard-видалення Складського юніта з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера під час hard-видалення Складського юніта з ID {id}: {ex.Message}");
        }
    }
}