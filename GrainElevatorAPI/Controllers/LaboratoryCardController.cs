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

[Route("api/laboratoryCard")]
[ApiController]
public class LaboratoryCardController : ControllerBase
{
    private readonly ILaboratoryCardService _laboratoryCardService;
    private readonly IMapper _mapper;
    private readonly ILogger<LaboratoryCardController> _logger;

    public LaboratoryCardController(ILaboratoryCardService laboratoryCardService, IMapper mapper, ILogger<LaboratoryCardController> logger)
    {
        _laboratoryCardService = laboratoryCardService;
        _mapper = mapper;
        _logger = logger;
    }
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    

    [HttpPost]
    //[Authorize(Roles = "Admin, Laboratory")]
    public async Task<ActionResult<LaboratoryCardDto>> CreateLaboratoryCard(LaboratoryCardCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var newLaboratoryCard = _mapper.Map<LaboratoryCard>(request);
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var createdLaboratoryCard = await _laboratoryCardService.CreateLaboratoryCardAsync(newLaboratoryCard, createdById, cancellationToken);
            return CreatedAtAction(nameof(GetLaboratoryCard), new { id = createdLaboratoryCard.Id },
                _mapper.Map<LaboratoryCardDto>(createdLaboratoryCard));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Лабораторної карточки: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Лабораторної карточки: {ex.Message}");
        }
    }


    [HttpGet]
    //[Authorize(Roles = "Admin, Laboratory")]
    public async Task<ActionResult<IEnumerable<LaboratoryCardDto>>> GetLaboratoryCard([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var laboratoryCards = await _laboratoryCardService.GetLaboratoryCards(page, size, cancellationToken);
            
            var laboratoryCardDtos = _mapper.Map<IEnumerable<LaboratoryCardDto>>(laboratoryCards);
            return Ok(laboratoryCardDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Лабораторних карточок: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Лабораторних карточок: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin, Laboratory")]
    public async Task<ActionResult<LaboratoryCardDto>> GetLaboratoryCard(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var laboratoryCard = await _laboratoryCardService.GetLaboratoryCardByIdAsync(id, cancellationToken);
            if (laboratoryCard == null)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<LaboratoryCardDto>(laboratoryCard));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Лабораторної карточки з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Лабораторної карточки з ID {id}: {ex.Message}");
        }
    }

    
    [HttpGet("search")]
    //[Authorize(Roles = "Admin, Laboratory")]
    public async Task<ActionResult<IEnumerable<LaboratoryCardDto>>> SearchLaboratoryCards(
        [FromQuery] int? id = null,
        [FromQuery] string? labCardNumber = null,
        [FromQuery] double? weedImpurity = null,
        [FromQuery] double? moisture = null,
        [FromQuery] bool? isProduction = null,
        
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
            var filteredLabCards = await _laboratoryCardService.SearchLaboratoryCards(
                id, 
                labCardNumber, 
                weedImpurity, 
                moisture, 
                isProduction, 
                arrivalDate, 
                vehicleNumber, 
                physicalWeight, 
                supplierId, 
                productId, 
                createdById, 
                removedAt, 
                page, 
                size,
                cancellationToken);

            var laboratoryCardDtos = _mapper.Map<IEnumerable<LaboratoryCardDto>>(filteredLabCards);
            return Ok(laboratoryCardDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Лабораторної карточки за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Лабораторної карточки за параметрами: {ex.Message}");
        }
    }
    

    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin, Laboratory")]
    public async Task<IActionResult> UpdateLaboratoryCard(int id, LaboratoryCardUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var laboratoryCardDb = await _laboratoryCardService.GetLaboratoryCardByIdAsync(id, cancellationToken);
            if (laboratoryCardDb == null)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }
            
            laboratoryCardDb.UpdateFromRequest(request);
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedLaboratoryCard = await _laboratoryCardService.UpdateLaboratoryCardAsync(laboratoryCardDb, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<LaboratoryCardDto>(updatedLaboratoryCard));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при при оновленні Лабораторної карточки з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Лабораторної карточки з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "Admin, Laboratory")]
    public async Task<IActionResult> SoftDeleteLaboratoryCard(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var laboratoryCardDb = await _laboratoryCardService.GetLaboratoryCardByIdAsync(id, cancellationToken);
            if (laboratoryCardDb == null)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedLaboratoryCard = await _laboratoryCardService.SoftDeleteLaboratoryCardAsync(laboratoryCardDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<LaboratoryCardDto>(removedLaboratoryCard));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Лабораторної карточки з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Лабораторної карточки з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "Admin, Laboratory")]
    public async Task<IActionResult> RestoreRemovedLaboratoryCard(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var laboratoryCardDb = await _laboratoryCardService.GetLaboratoryCardByIdAsync(id, cancellationToken);
            if (laboratoryCardDb == null)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredLaboratoryCard = await _laboratoryCardService.RestoreRemovedLaboratoryCardAsync(laboratoryCardDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<LaboratoryCardDto>(restoredLaboratoryCard));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Лабораторної карточки з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Лабораторної карточки з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteLaboratoryCard(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _laboratoryCardService.DeleteLaboratoryCardAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Лабораторної карточки з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Лабораторної карточки з ID {id}: {ex.Message}");
        }
    }
}
  

