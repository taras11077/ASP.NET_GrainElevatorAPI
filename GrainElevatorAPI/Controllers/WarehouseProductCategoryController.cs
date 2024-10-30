using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Extensions;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[Route("api/warehouseProductCategory")]
[ApiController]
public class WarehouseProductCategoryController: ControllerBase

{
    private readonly IWarehouseProductCategoryService _warehouseProductCategoryService;
    private readonly IMapper _mapper;
    private readonly ILogger<WarehouseProductCategoryController> _logger;

    public WarehouseProductCategoryController(IWarehouseProductCategoryService warehouseProductCategoryService, IMapper mapper, ILogger<WarehouseProductCategoryController> logger)
    {
        _warehouseProductCategoryService = warehouseProductCategoryService;
        _mapper = mapper;
        _logger = logger;
    }
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    [HttpPost]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<WarehouseProductCategory>> CreateWarehouseProductCategory(WarehouseProductCategoryCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var newWarehouseProductCategory = _mapper.Map<WarehouseProductCategory>(request);
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var createdWarehouseProductCategory = await _warehouseProductCategoryService.CreateWarehouseProductCategoryAsync(newWarehouseProductCategory, createdById, cancellationToken);
            return CreatedAtAction(nameof(GetWarehouseProductCategory), new { id = createdWarehouseProductCategory.Id },
                _mapper.Map<WarehouseProductCategoryDto>(createdWarehouseProductCategory));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Категорії продукції: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Категорії продукції: {ex.Message}");
        }
    }


    [HttpGet]
    //[Authorize(Roles = "Admin, Accountant")]
    public ActionResult<IEnumerable<WarehouseProductCategory>> GetWarehouseProductCategory([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var warehouseProductCategories = _warehouseProductCategoryService.GetWarehouseProductCategories(page, size);
            return Ok(_mapper.Map<IEnumerable<WarehouseProductCategoryDto>>(warehouseProductCategories));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Категорій продукції: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Категорій продукції: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<WarehouseProductCategory>> GetWarehouseProductCategory(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var warehouseProductCategory = await _warehouseProductCategoryService.GetWarehouseProductCategoryByIdAsync(id, cancellationToken);
            if (warehouseProductCategory == null)
            {
                return NotFound($"Категорії продукції з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<WarehouseProductCategoryDto>(warehouseProductCategory));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Категорії продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Категорії продукції з ID {id}: {ex.Message}");
        }
    }

    
    [HttpGet("search")]
    //[Authorize(Roles = "Admin, Accountant")]
    public ActionResult<IEnumerable<WarehouseProductCategoryDto>> SearchWarehouseProductCategories(
        [FromQuery] int? id = null,
        [FromQuery] string? title = null,
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
            var filteredCategories = _warehouseProductCategoryService.SearchWarehouseProductCategories(
                id, 
                title, 
                supplierId, 
                productId, 
                createdById, 
                removedAt, 
                page, 
                size);

            return Ok(_mapper.Map<IEnumerable<WarehouseProductCategoryDto>>(filteredCategories));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Категорії продукції за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Категорії продукції за параметрами: {ex.Message}");
        }
    }
    

    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> UpdateWarehouseProductCategory(int id, WarehouseProductCategoryUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var warehouseProductCategoryDb = await _warehouseProductCategoryService.GetWarehouseProductCategoryByIdAsync(id, cancellationToken);
            if (warehouseProductCategoryDb == null)
            {
                return NotFound($"Категорії продукції з ID {id} не знайдено.");
            }
            
            warehouseProductCategoryDb.UpdateFromRequest(request);
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedWarehouseProductCategory = await _warehouseProductCategoryService.UpdateWarehouseProductCategoryAsync(warehouseProductCategoryDb, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<WarehouseProductCategoryDto>(updatedWarehouseProductCategory));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при при оновленні Категорії продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Категорії продукції з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> SoftDeleteLaboratoryCar(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var warehouseProductCategoryDb = await _warehouseProductCategoryService.GetWarehouseProductCategoryByIdAsync(id, cancellationToken);
            if (warehouseProductCategoryDb == null)
            {
                return NotFound($"Категорії продукції з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedWarehouseProductCategory = await _warehouseProductCategoryService.SoftDeleteWarehouseProductCategoryAsync(warehouseProductCategoryDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<WarehouseProductCategoryDto>(removedWarehouseProductCategory));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Категорії продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Категорії продукції з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> RestoreRemovedWarehouseProductCategory(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var warehouseProductCategoryDb = await _warehouseProductCategoryService.GetWarehouseProductCategoryByIdAsync(id, cancellationToken);
            if (warehouseProductCategoryDb == null)
            {
                return NotFound($"Категорії продукції з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredWarehouseProductCategory = await _warehouseProductCategoryService.RestoreRemovedWarehouseProductCategoryAsync(warehouseProductCategoryDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<WarehouseProductCategoryDto>(restoredWarehouseProductCategory));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Категорії продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Категорії продукції з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteWarehouseProductCategory(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _warehouseProductCategoryService.DeleteWarehouseProductCategoryAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Категорії продукції з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Категорії продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Категорії продукції з ID {id}: {ex.Message}");
        }
    }
}