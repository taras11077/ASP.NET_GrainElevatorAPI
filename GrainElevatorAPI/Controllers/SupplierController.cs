using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/supplier")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    private readonly IMapper _mapper;

    public SupplierController(ISupplierService supplierService, IMapper mapper)
    {
        _supplierService = supplierService;
        _mapper = mapper;
    }
    
    // POST: api/Supplier
    [HttpPost]
    public async Task<ActionResult<Supplier>> PostSupplier(SupplierCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var newSupplier = _mapper.Map<Supplier>(request);
                
            newSupplier.CreatedAt = DateTime.UtcNow;
            newSupplier.CreatedById = (int)HttpContext.Session.GetInt32("EmployeeId");
            
            var createdSupplier = await _supplierService.AddSupplierAsync(newSupplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.Id }, _mapper.Map<SupplierDTO>(createdSupplier));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // GET: api/Supplier
    [HttpGet]
    public ActionResult<IEnumerable<Supplier>> GetSuppliers([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var suppliers = _supplierService.GetSuppliers(page, size);
            return Ok(_mapper.Map<IEnumerable<SupplierDTO>>(suppliers));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // GET: api/Supplier/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Supplier>> GetSupplier(int id)
    {
        try
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound($"Постачальника з ID {id} не знайдено.");
            }
            return Ok(_mapper.Map<SupplierDTO>(supplier));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // PUT: api/Supplier/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSupplier(int id, SupplierCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var supplierDb = await _supplierService.GetSupplierByIdAsync(id);
        
            supplierDb.Title = request.Title;
        
            var updatedSupplier = await _supplierService.UpdateSupplierAsync(supplierDb);
        
            if (updatedSupplier == null)
            {
                return NotFound($"Постачальника з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<SupplierDTO>(updatedSupplier));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    
    // DELETE: api/Supplier/5
    [HttpDelete("{id}")]
    //[Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        try
        {
            var success = await _supplierService.DeleteSupplierAsync(id);
            if (!success)
            {
                return NotFound($"Постачальника з ID {id} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
     
     // Patch: api/Supplier/5
        [HttpPatch("{id}/soft-remove")]
        //[Authorize(Roles = "admin, laboratory")]
        public async Task<IActionResult> SoftDeleteSupplier(int id)
        {
            try
            {
                var supplierDb = await _supplierService.GetSupplierByIdAsync(id);
                
                supplierDb.Removed = true;
                supplierDb.RemovedAt = DateTime.UtcNow;
                supplierDb.RemovedById = (int)HttpContext.Session.GetInt32("EmployeeId");
                
                var removedSupplier = await _supplierService.UpdateSupplierAsync(supplierDb);
                
                if (removedSupplier == null)
                {
                    return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
                }

                return Ok(_mapper.Map<SupplierDTO>(removedSupplier));
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
        
        // Patch: api/Supplier/5
        [HttpPatch("{id}/restore")]
        //[Authorize(Roles = "admin, laboratory")]
        public async Task<IActionResult> RestoreRemovedSupplier(int id)
        {
            try
            {
                var supplierDb = await _supplierService.GetSupplierByIdAsync(id);
                
                supplierDb.Removed = false;
                supplierDb.RestoredAt = DateTime.UtcNow;
                supplierDb.RestoreById = (int)HttpContext.Session.GetInt32("EmployeeId");
                
                var restoredSupplier = await _supplierService.UpdateSupplierAsync(supplierDb);
                
                if (restoredSupplier == null)
                {
                    return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
                }

                return Ok(_mapper.Map<SupplierDTO>(restoredSupplier));
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
    
    
    // GET: api/Supplier/search?title=Khortytsia 
    [HttpGet("search")]
    public ActionResult<IEnumerable<Supplier>> SearchRoles(string title)
    {
        try
        {
            var suppliers = _supplierService.SearchSupplier(title);
            return Ok(_mapper.Map<IEnumerable<SupplierDTO>>(suppliers));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    } 
}

