using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;
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


	[HttpPost]
	public async Task<ActionResult<Supplier>> CreateSupplier(SupplierCreateRequest request)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		try
		{
			var newSupplier = _mapper.Map<Supplier>(request);
			var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();

			var createdSupplier = await _supplierService.AddSupplierAsync(newSupplier, createdById);
			return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.Id },
				_mapper.Map<SupplierDto>(createdSupplier));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при створенні Постачальника: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при створенні Постачальника: {ex.Message}");
		}
	}


	[HttpGet]
	public ActionResult<IEnumerable<Supplier>> GetSuppliers([FromQuery] int page = 1, [FromQuery] int size = 10)
	{
		try
		{
			var suppliers = _supplierService.GetSuppliers(page, size);
			return Ok(_mapper.Map<IEnumerable<SupplierDto>>(suppliers));
		}
		catch (Exception ex)
		{
			_logger.LogError($"Внутрішня помилка сервера при отриманні всіх Постачальників: {ex.Message}");
			return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Постачальників: {ex.Message}");
		}
	}


	[HttpGet("{id}")]
	public async Task<ActionResult<Supplier>> GetSupplier(int id)
	{
		try
		{
			var supplier = await _supplierService.GetSupplierByIdAsync(id);
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
	public ActionResult<IEnumerable<Supplier>> SearchSuppliers(string title)
	{
		try
		{
			var suppliers = _supplierService.SearchSupplier(title);
			return Ok(_mapper.Map<IEnumerable<SupplierDto>>(suppliers));
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
			var supplierDb = await _supplierService.GetSupplierByIdAsync(id);
			if (supplierDb == null) return NotFound($"Постачальника з ID {id} не знайдено.");
			
			supplierDb.Title = request.Title;
			var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var updatedSupplier = await _supplierService.UpdateSupplierAsync(supplierDb, modifiedById);
			
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
			var supplierDb = await _supplierService.GetSupplierByIdAsync(id);
			if (supplierDb == null) return NotFound($"Постачальника з ID {id} не знайдено.");

			var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var removedSupplier = await _supplierService.SoftDeleteSupplierAsync(supplierDb, removedById);
			
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
			var supplierDb = await _supplierService.GetSupplierByIdAsync(id);
			if (supplierDb == null) return NotFound($"Постачальника з ID {id} не знайдено.");
			
			var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
			var restoredSupplier = await _supplierService.RestoreRemovedSupplierAsync(supplierDb, restoredById);
			
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
			var success = await _supplierService.DeleteSupplierAsync(id);
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