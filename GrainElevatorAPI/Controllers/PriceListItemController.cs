using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

public class PriceListItemController: ControllerBase

{
    private readonly IPriceListItemService _priceListItemService;
    private readonly IMapper _mapper;
    private readonly ILogger<PriceListItemController> _logger;

    public PriceListItemController(IPriceListItemService priceListItemService, IMapper mapper, ILogger<PriceListItemController> logger)
    {
        _priceListItemService = priceListItemService;
        _mapper = mapper;
        _logger = logger;
    }
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    [HttpPost]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<PriceListItemDto>> CreatePriceListItem(PriceListItemCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var newPriceListItem = _mapper.Map<PriceListItem>(request);
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var createdPriceListItem = await _priceListItemService.CreatePriceListItemAsync(newPriceListItem, createdById, cancellationToken);
            return CreatedAtAction(nameof(GetPriceListItem), new { id = createdPriceListItem.Id },
                _mapper.Map<PriceListItemDto>(createdPriceListItem));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Елемента прайс-листа: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Елемента прайс-листа: {ex.Message}");
        }
    }


    [HttpGet]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<IEnumerable<PriceListItemDto>>> GetPriceListItem([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var priceListItems = await _priceListItemService.GetPriceListItems(page, size, cancellationToken);
            
            var priceListItemDtos = _mapper.Map<IEnumerable<PriceListItemDto>>(priceListItems);
            return Ok(priceListItemDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Елементів прайс-листа: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Елементів прайс-листа: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<PriceListItemDto>> GetPriceListItem(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var PriceListItem = await _priceListItemService.GetPriceListItemByIdAsync(id, cancellationToken);
            if (PriceListItem == null)
            {
                return NotFound($"Елемента прайс-листа з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<PriceListItemDto>(PriceListItem));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Елемента прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Елемента прайс-листа з ID {id}: {ex.Message}");
        }
    }

    
    [HttpGet("search")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<IEnumerable<PriceListItemDto>>> SearchPriceListItems(
        [FromQuery] int? id = null,
        [FromQuery] int? technologicalOperationId = null,
        [FromQuery] decimal? operationPrice = null,
        [FromQuery] int? priceListId = null,
        [FromQuery] int? createdById = null,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            // передаємо параметри у сервіс для фільтрації
            var filteredCategories = await _priceListItemService.SearchPriceListItems(
                id, 
                technologicalOperationId,
                operationPrice, 
                priceListId,
                createdById,
                page,
                size,
                cancellationToken);

            var priceListItemDtos = _mapper.Map<IEnumerable<PriceListItemDto>>(filteredCategories);
            return Ok(priceListItemDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Елемента прайс-листа за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Елемента прайс-листа за параметрами: {ex.Message}");
        }
    }
    

    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> UpdatePriceListItem(int id, PriceListItemUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var priceListItemDb = await _priceListItemService.GetPriceListItemByIdAsync(id, cancellationToken);
            if (priceListItemDb == null)
            {
                return NotFound($"Елемента прайс-листа з ID {id} не знайдено.");
            }
            
            priceListItemDb.UpdateFromRequest(request);
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedPriceListItem = await _priceListItemService.UpdatePriceListItemAsync(priceListItemDb, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<PriceListItemDto>(updatedPriceListItem));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при при оновленні Елемента прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Елемента прайс-листа з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> SoftDeleteLaboratoryCar(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var priceListItemDb = await _priceListItemService.GetPriceListItemByIdAsync(id, cancellationToken);
            if (priceListItemDb == null)
            {
                return NotFound($"Елемента прайс-листа з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedPriceListItem = await _priceListItemService.SoftDeletePriceListItemAsync(priceListItemDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<PriceListItemDto>(removedPriceListItem));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Елемента прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Елемента прайс-листа з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> RestoreRemovedPriceListItem(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var priceListItemDb = await _priceListItemService.GetPriceListItemByIdAsync(id, cancellationToken);
            if (priceListItemDb == null)
            {
                return NotFound($"Елемента прайс-листа з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredPriceListItem = await _priceListItemService.RestoreRemovedPriceListItemAsync(priceListItemDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<PriceListItemDto>(restoredPriceListItem));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Елемента прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Елемента прайс-листа з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePriceListItem(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _priceListItemService.DeletePriceListItemAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Елемента прайс-листа з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Елемента прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Елемента прайс-листа з ID {id}: {ex.Message}");
        }
    }
}