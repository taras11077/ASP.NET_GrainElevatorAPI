using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[Route("api/price-list")]
[ApiController]
public class PriceListController : ControllerBase

{
    private readonly IPriceListService _priceListService;
    private readonly IMapper _mapper;
    private readonly ILogger<PriceListController> _logger;

    public PriceListController(IPriceListService priceListService, IMapper mapper, ILogger<PriceListController> logger)
    {
        _priceListService = priceListService;
        _mapper = mapper;
        _logger = logger;
    }

    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<PriceListDto>> CreatePriceList(PriceListCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var cancellationToken = GetCancellationToken();
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            //var priceListItems = _mapper.Map<IEnumerable<PriceListItem>>(request.PriceListItems);
            var priceListItems = new List<PriceListItem>(); // TODO
            
            var createdPriceList = await _priceListService.CreatePriceListAsync(request.ProductTitle, priceListItems, createdById, cancellationToken);
            return CreatedAtAction(nameof(GetPriceList), new { id = createdPriceList.Id },
                _mapper.Map<PriceListDto>(createdPriceList));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Прайс-листа: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Прайс-листа: {ex.Message}");
        }
    }


    [HttpGet]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<IEnumerable<PriceListDto>>> GetPriceList([FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var priceLists = await _priceListService.GetPriceLists(page, size, cancellationToken);

            var priceListDtos = _mapper.Map<IEnumerable<PriceListDto>>(priceLists);
            return Ok(priceListDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Прайс-листів: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Прайс-листів: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Accountant,CEO")]
    public async Task<ActionResult<PriceListDto>> GetPriceList(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var PriceList = await _priceListService.GetPriceListByIdAsync(id, cancellationToken);
            if (PriceList == null)
            {
                return NotFound($"Прайс-листа з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<PriceListDto>(PriceList));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Прайс-листа з ID {id}: {ex.Message}");
        }
    }


    [HttpGet("search")]
    [Authorize(Roles = "Admin,Accountant,CEO")]
    public async Task<ActionResult<IEnumerable<PriceListDto>>> SearchPriceLists(
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

            var (filteredPriceLists, totalCount) = await _priceListService.SearchPriceLists(
                productTitle,
                createdByName,
                page,
                size,
                sortField,
                sortOrder,
                cancellationToken);

            var priceListDtos = _mapper.Map<IEnumerable<PriceListDto>>(filteredPriceLists);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            
            return Ok(priceListDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Прайс-листів за параметрами: {ex.Message}");
            return StatusCode(500,
                $"Внутрішня помилка сервера при отриманні Прайс-листів за параметрами: {ex.Message}");
        }
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> UpdatePriceList(int id, PriceListUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var cancellationToken = GetCancellationToken();
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var updatedPriceList = await _priceListService.UpdatePriceListAsync(id, request.ProductId, request.PriceListItemIds, modifiedById, cancellationToken);

            return Ok(_mapper.Map<PriceListDto>(updatedPriceList));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при при оновленні Прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Прайс-листа з ID {id}: {ex.Message}");
        }
    }


    [HttpPatch("{id}/soft-remove")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> SoftDeleteLaboratoryCar(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var priceListDb = await _priceListService.GetPriceListByIdAsync(id, cancellationToken);
            if (priceListDb == null)
            {
                return NotFound($"Прайс-листа з ID {id} не знайдено.");
            }

            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedPriceList =
                await _priceListService.SoftDeletePriceListAsync(priceListDb, removedById, cancellationToken);

            return Ok(_mapper.Map<PriceListDto>(removedPriceList));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Прайс-листа з ID {id}: {ex.Message}");
        }
    }


    [HttpPatch("{id}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RestoreRemovedPriceList(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var priceListDb = await _priceListService.GetPriceListByIdAsync(id, cancellationToken);
            if (priceListDb == null)
            {
                return NotFound($"Прайс-листа з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredPriceList =
                await _priceListService.RestoreRemovedPriceListAsync(priceListDb, restoredById, cancellationToken);

            return Ok(_mapper.Map<PriceListDto>(restoredPriceList));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Прайс-листа з ID {id}: {ex.Message}");
        }
    }


    [HttpDelete("{id}/hard-remove")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePriceList(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _priceListService.DeletePriceListAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Прайс-листа з ID {id} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Прайс-листа з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Прайс-листа з ID {id}: {ex.Message}");
        }
    }
}
