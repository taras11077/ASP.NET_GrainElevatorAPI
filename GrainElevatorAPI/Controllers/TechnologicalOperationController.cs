using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/technological-operation")]
public class TechnologicalOperationController : ControllerBase
{
	private readonly ITechnologicalOperationService _technologicalOperationService;
	private readonly IMapper _mapper;
	private readonly ILogger<TechnologicalOperationController> _logger;
	
	public TechnologicalOperationController(ITechnologicalOperationService technologicalOperationService, IMapper mapper, ILogger<TechnologicalOperationController> logger)
	{
		_technologicalOperationService = technologicalOperationService;
		_mapper = mapper;
		_logger = logger;
	}

	private CancellationToken GetCancellationToken()
	{
		return HttpContext.RequestAborted;
	}

	[HttpPost]
	public async Task<ActionResult<TechnologicalOperationDto>> CreateTechnologicalOperation(TechnologicalOperationCreateRequest request)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		try
		{
			var cancellationToken = GetCancellationToken();
			var newTechnologicalOperation = _mapper.Map<TechnologicalOperation>(request);
			var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();

			var createdTechnologicalOperation = await _technologicalOperationService.CreateTechnologicalOperationAsync(newTechnologicalOperation, createdById, cancellationToken);
			return CreatedAtAction(nameof(GetTechnologicalOperationById), new { id = createdTechnologicalOperation.Id },
				_mapper.Map<TechnologicalOperationDto>(createdTechnologicalOperation));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при створенні Технологічної операції: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при створенні Технологічної операції: {ex.Message}");
		}
	}


	[HttpGet]
	public async Task<ActionResult<IEnumerable<TechnologicalOperationDto>>> GetTechnologicalOperations([FromQuery] int page = 1, [FromQuery] int size = 10)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var technologicalOperations = await _technologicalOperationService.GetTechnologicalOperations(page, size, cancellationToken);
			
			var technologicalOperationDtos = _mapper.Map<IEnumerable<TechnologicalOperationDto>>(technologicalOperations);
			return Ok(technologicalOperationDtos);
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при отриманні всіх Технологічних операцій: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Технологічних операцій: {ex.Message}");
		}
	}


	[HttpGet("{id}")]
	public async Task<ActionResult<TechnologicalOperationDto>> GetTechnologicalOperationById(int id)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var technologicalOperation = await _technologicalOperationService.GetTechnologicalOperationByIdAsync(id, cancellationToken);
			if (technologicalOperation == null) return NotFound($"Технологічної операції з ID {id} не знайдено.");
			
			return Ok(_mapper.Map<TechnologicalOperationDto>(technologicalOperation));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при отриманні Технологічної операції з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при отриманні Технологічної операції з ID {id}: {ex.Message}");
		}
	}

	[HttpGet("search")]
	public async Task<ActionResult<IEnumerable<TechnologicalOperationDto>>> SearchTechnologicalOperations(string title)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var TechnologicalOperations = await _technologicalOperationService.SearchTechnologicalOperation(title, cancellationToken);
			
			var TechnologicalOperationDtos = _mapper.Map<IEnumerable<TechnologicalOperationDto>>(TechnologicalOperations);
			return Ok(TechnologicalOperationDtos);
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при отриманні Технологічної операції за назвою: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при отриманні Технологічної операції за назвою: {ex.Message}");
		}
	}
	
	
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateTechnologicalOperation(int id, TechnologicalOperationCreateRequest request)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		try
		{
			var cancellationToken = GetCancellationToken();
			var technologicalOperationDb = await _technologicalOperationService.GetTechnologicalOperationByIdAsync(id, cancellationToken);
			if (technologicalOperationDb == null) return NotFound($"Технологічної операції з ID {id} не знайдено.");
			
			technologicalOperationDb.Title = request.Title;
			var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var updatedTechnologicalOperation = await _technologicalOperationService.UpdateTechnologicalOperationAsync(technologicalOperationDb, modifiedById, cancellationToken);
			
			return Ok(_mapper.Map<TechnologicalOperationDto>(updatedTechnologicalOperation));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при оновленні Технологічної операції з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при оновленні Технологічної операції з ID {id}: {ex.Message}");
		}
	}

	
	[HttpPatch("{id}/soft-remove")]
	//[Authorize(Roles = "admin, laboratory")]
	public async Task<IActionResult> SoftDeleteTechnologicalOperation(int id)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var technologicalOperationDb = await _technologicalOperationService.GetTechnologicalOperationByIdAsync(id, cancellationToken);
			if (technologicalOperationDb == null) return NotFound($"Технологічної операції з ID {id} не знайдено.");

			var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var removedTechnologicalOperation = await _technologicalOperationService.SoftDeleteTechnologicalOperationAsync(technologicalOperationDb, removedById, cancellationToken);
			
			return Ok(_mapper.Map<TechnologicalOperationDto>(removedTechnologicalOperation));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при soft-видаленні Технологічної операції з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при видаленні Технологічної операції з ID {id}: {ex.Message}");
		}
	}


	[HttpPatch("{id}/restore")]
	//[Authorize(Roles = "admin, laboratory")]
	public async Task<IActionResult> RestoreRemovedTechnologicalOperation(int id)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var technologicalOperationDb = await _technologicalOperationService.GetTechnologicalOperationByIdAsync(id, cancellationToken);
			if (technologicalOperationDb == null) return NotFound($"Технологічної операції з ID {id} не знайдено.");
			
			var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var restoredTechnologicalOperation = await _technologicalOperationService.RestoreRemovedTechnologicalOperationAsync(technologicalOperationDb, restoredById, cancellationToken);
			
			return Ok(_mapper.Map<TechnologicalOperationDto>(restoredTechnologicalOperation));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при відновленні Технологічної операції з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при відновленні Технологічної операції з ID {id}: {ex.Message}");
		}
	}
	
	[HttpDelete("{id}/hard-remove")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> DeleteTechnologicalOperation(int id)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var success = await _technologicalOperationService.DeleteTechnologicalOperationAsync(id, cancellationToken);
			if (!success) return NotFound($"Технологічної операції з ID {id} не знайдено.");

			return NoContent();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при hard-видаленні Технологічної операції з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Технологічної операції з ID {id}: {ex.Message}");
		}
	}
	
}