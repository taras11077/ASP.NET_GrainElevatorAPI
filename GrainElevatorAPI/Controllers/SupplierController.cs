using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/supplier")]
public class SupplierController : ControllerBase
{
	private readonly ISupplierService _supplierService;
	private readonly IMapper _mapper;
	private readonly ILogger<SupplierController> _logger;
	
	public SupplierController(ISupplierService supplierService, IMapper mapper, ILogger<SupplierController> logger)
	{
		_supplierService = supplierService;
		_mapper = mapper;
		_logger = logger;
	}

	private CancellationToken GetCancellationToken()
	{
		return HttpContext.RequestAborted;
	}

	[HttpPost]
	public async Task<ActionResult<Supplier>> CreateSupplier(SupplierCreateRequest request)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		try
		{
			var cancellationToken = GetCancellationToken();
			var newSupplier = _mapper.Map<Supplier>(request);
			var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();

			var createdSupplier = await _supplierService.CreateSupplierAsync(newSupplier, createdById, cancellationToken);
			return CreatedAtAction(nameof(GetSupplierById), new { id = createdSupplier.Id },
				_mapper.Map<SupplierDto>(createdSupplier));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при створенні Постачальника: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при створенні Постачальника: {ex.Message}");
		}
	}


	[HttpGet]
	public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers([FromQuery] int page = 1, [FromQuery] int size = 10)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var suppliers = await _supplierService.GetSuppliers(page, size, cancellationToken);
			
			var supplierDtos = _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
			return Ok(supplierDtos);
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при отриманні всіх Постачальників: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Постачальників: {ex.Message}");
		}
	}


	[HttpGet("{id}")]
	public async Task<ActionResult<Supplier>> GetSupplierById(int id)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var supplier = await _supplierService.GetSupplierByIdAsync(id, cancellationToken);
			if (supplier == null) return NotFound($"Постачальника з ID {id} не знайдено.");
			
			return Ok(_mapper.Map<SupplierDto>(supplier));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при отриманні Постачальника з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при отриманні Постачальника з ID {id}: {ex.Message}");
		}
	}

	[HttpGet("search")]
	public async Task<ActionResult<IEnumerable<Supplier>>> SearchSuppliers(string title)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var suppliers = await _supplierService.SearchSupplier(title, cancellationToken);
			
			var supplierDtos = _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
			return Ok(supplierDtos);
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при отриманні Постачальника за назвою: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при отриманні Постачальника за назвою: {ex.Message}");
		}
	}
	
	
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateSupplier(int id, SupplierCreateRequest request)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		try
		{
			var cancellationToken = GetCancellationToken();
			var supplierDb = await _supplierService.GetSupplierByIdAsync(id, cancellationToken);
			if (supplierDb == null) return NotFound($"Постачальника з ID {id} не знайдено.");
			
			supplierDb.Title = request.Title;
			var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var updatedSupplier = await _supplierService.UpdateSupplierAsync(supplierDb, modifiedById, cancellationToken);
			
			return Ok(_mapper.Map<SupplierDto>(updatedSupplier));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при оновленні Постачальника з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при оновленні Постачальника з ID {id}: {ex.Message}");
		}
	}

	
	[HttpPatch("{id}/soft-remove")]
	//[Authorize(Roles = "admin, laboratory")]
	public async Task<IActionResult> SoftDeleteSupplier(int id)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var supplierDb = await _supplierService.GetSupplierByIdAsync(id, cancellationToken);
			if (supplierDb == null) return NotFound($"Постачальника з ID {id} не знайдено.");

			var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var removedSupplier = await _supplierService.SoftDeleteSupplierAsync(supplierDb, removedById, cancellationToken);
			
			return Ok(_mapper.Map<SupplierDto>(removedSupplier));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при soft-видаленні Постачальника з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при видаленні Постачальника з ID {id}: {ex.Message}");
		}
	}


	[HttpPatch("{id}/restore")]
	//[Authorize(Roles = "admin, laboratory")]
	public async Task<IActionResult> RestoreRemovedSupplier(int id)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var supplierDb = await _supplierService.GetSupplierByIdAsync(id, cancellationToken);
			if (supplierDb == null) return NotFound($"Постачальника з ID {id} не знайдено.");
			
			var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var restoredSupplier = await _supplierService.RestoreRemovedSupplierAsync(supplierDb, restoredById, cancellationToken);
			
			return Ok(_mapper.Map<SupplierDto>(restoredSupplier));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при відновленні Постачальника з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при відновленні Постачальника з ID {id}: {ex.Message}");
		}
	}
	
	[HttpDelete("{id}/hard-remove")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> DeleteSupplier(int id)
	{
		try
		{
			var cancellationToken = GetCancellationToken();
			var success = await _supplierService.DeleteSupplierAsync(id, cancellationToken);
			if (!success) return NotFound($"Постачальника з ID {id} не знайдено.");

			return NoContent();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при hard-видаленні Постачальника з ID {id}: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Постачальника з ID {id}: {ex.Message}");
		}
	}
	
}