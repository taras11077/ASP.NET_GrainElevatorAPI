using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Services;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Extensions;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Controllers;

[Route("api/laboratoryCard")]
[ApiController]
public class LaboratoryCardController : ControllerBase
{
    private readonly ILaboratoryCardService _laboratoryCardService;
    private readonly IMapper _mapper;

    public LaboratoryCardController(ILaboratoryCardService laboratoryCardService, IMapper mapper)
    {
        _laboratoryCardService = laboratoryCardService;
        _mapper = mapper;
    }
    

    [HttpPost]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<ActionResult<LaboratoryCard>> PostLaboratoryCard(LaboratoryCardCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var newLaboratoryCard = _mapper.Map<LaboratoryCard>(request);
            
            newLaboratoryCard.CreatedAt = DateTime.UtcNow;
            newLaboratoryCard.CreatedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var createdILaboratoryCard = await _laboratoryCardService.AddLaboratoryCardAsync(newLaboratoryCard);
            return CreatedAtAction(nameof(GetLaboratoryCard), new { id = createdILaboratoryCard.Id },
                _mapper.Map<LaboratoryCardDTO>(createdILaboratoryCard));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpGet]
    //[Authorize(Roles = "admin, laboratory")]
    public ActionResult<IEnumerable<LaboratoryCard>> GetLaboratoryCard([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var laboratoryCards = _laboratoryCardService.GetLaboratoryCards(page, size);
            return Ok(_mapper.Map<IEnumerable<LaboratoryCardDTO>>(laboratoryCards));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<ActionResult<LaboratoryCard>> GetLaboratoryCard(int id)
    {
        try
        {
            var laboratoryCard = await _laboratoryCardService.GetLaboratoryCardByIdAsync(id);
            if (laboratoryCard == null)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<LaboratoryCardDTO>(laboratoryCard));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpPut("{id}")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> PutLaboratoryCard(int id, LaboratoryCardUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var laboratoryCardDb = await _laboratoryCardService.GetLaboratoryCardByIdAsync(id);
            
            laboratoryCardDb.UpdateFromRequest(request);
            
            laboratoryCardDb.ModifiedAt = DateTime.UtcNow;
            laboratoryCardDb.ModifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();

            var updatedLaboratoryCard = await _laboratoryCardService.UpdateLaboratoryCardAsync(laboratoryCardDb);

            if (updatedLaboratoryCard == null)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<LaboratoryCardDTO>(updatedLaboratoryCard));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    

    [HttpDelete("{id}/hard-remove")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteLaboratoryCar(int id)
    {
        try
        {
            var success = await _laboratoryCardService.DeleteLaboratoryCardAsync(id);
            if (!success)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> SoftDeleteLaboratoryCar(int id)
    {
        try
        {
            var laboratoryCardDb = await _laboratoryCardService.GetLaboratoryCardByIdAsync(id);
            
            laboratoryCardDb.RemovedAt = DateTime.UtcNow;
            laboratoryCardDb.RemovedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var removedLaboratoryCard = await _laboratoryCardService.UpdateLaboratoryCardAsync(laboratoryCardDb);
            
            if (removedLaboratoryCard == null)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<LaboratoryCardDTO>(removedLaboratoryCard));
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> RestoreRemovedLaboratoryCard(int id)
    {
        try
        {
            var laboratoryCardDb = await _laboratoryCardService.GetLaboratoryCardByIdAsync(id);

            laboratoryCardDb.RemovedAt = null;
            laboratoryCardDb.RestoredAt = DateTime.UtcNow;
            laboratoryCardDb.RestoreById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var restorededLaboratoryCard = await _laboratoryCardService.UpdateLaboratoryCardAsync(laboratoryCardDb);
            
            if (restorededLaboratoryCard == null)
            {
                return NotFound($"Лабораторної карточки з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<LaboratoryCardDTO>(restorededLaboratoryCard));
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    
    [HttpGet("search")]
    //[Authorize(Roles = "admin, laboratory")]
    public ActionResult<IEnumerable<LaboratoryCardDTO>> SearchLaboratoryCards(
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
            IQueryable<LaboratoryCard> query = _laboratoryCardService.GetLaboratoryCards(page, size)
                            .Include(lc => lc.InputInvoice)
                            .AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(lc => lc.Id == id);
            }
            
            if (!string.IsNullOrEmpty(labCardNumber))
            {
                query = query.Where(lc => lc.LabCardNumber == labCardNumber);
            }

            if (arrivalDate.HasValue)
            {
                query = query.Where(lc => lc.InputInvoice.ArrivalDate.Date == arrivalDate.Value.Date);
            }
                
            
            if (!string.IsNullOrEmpty(vehicleNumber))
                query = query.Where(lc => lc.InputInvoice.VehicleNumber == vehicleNumber);
            
            if (physicalWeight.HasValue)
                query = query.Where(lc => lc.InputInvoice.PhysicalWeight == physicalWeight.Value);
            
            if (supplierId.HasValue)
                query = query.Where(lc => lc.InputInvoice.SupplierId == supplierId.Value);

            if (productId.HasValue)
                query = query.Where(lc => lc.InputInvoice.ProductId == productId.Value);
            

            if (weedImpurity.HasValue)
                query = query.Where(lc => lc.WeedImpurity == weedImpurity.Value);

            if (moisture.HasValue)
                query = query.Where(lc => lc.Moisture == moisture.Value);
            
            if (isProduction.HasValue)
                query = query.Where(lc => lc.IsProduction == isProduction.Value);
            
            if (createdById.HasValue)
                query = query.Where(lc => lc.CreatedById == createdById.Value);

            if (removedAt.HasValue)
                query = query.Where(lc => lc.RemovedAt == removedAt.Value);

            var result = query.ToList();
            return Ok(_mapper.Map<IEnumerable<LaboratoryCardDTO>>(result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    
    
}
  

