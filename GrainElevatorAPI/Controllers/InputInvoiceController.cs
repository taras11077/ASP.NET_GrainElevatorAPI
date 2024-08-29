using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Extensions;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers
{
    [Route("api/input-invoice")]
    [ApiController]
    public class InputInvoiceController : ControllerBase
    {
        private readonly IInputInvoiceService _inputInvoiceService;
        private readonly IMapper _mapper;

        public InputInvoiceController(IInputInvoiceService inputInvoiceService, IMapper mapper)
        {
            _inputInvoiceService = inputInvoiceService;
            _mapper = mapper;
        }
        
        // POST: api/InputInvoice
        [HttpPost]
        //[Authorize(Roles = "admin, laboratory")]
        public async Task<ActionResult<InputInvoice>> PostInputInvoice(InputInvoiceCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var newInputInvoice = _mapper.Map<InputInvoice>(request);
                
                newInputInvoice.CreatedAt = DateTime.UtcNow;
                newInputInvoice.CreatedById = (int)HttpContext.Session.GetInt32("EmployeeId");
                
                var createdInputInvoice = await _inputInvoiceService.AddInputInvoiceAsync(newInputInvoice);
                return CreatedAtAction(nameof(GetInputInvoice), new { id = createdInputInvoice.Id },
                    _mapper.Map<InputInvoiceDTO>(createdInputInvoice));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }

        // GET: api/InputInvoice
        [HttpGet]
        //[Authorize(Roles = "admin, laboratory")]
        public ActionResult<IEnumerable<InputInvoice>> GetInputInvoice([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var inputInvoices = _inputInvoiceService.GetInputInvoices(page, size);
                return Ok(_mapper.Map<IEnumerable<InputInvoiceDTO>>(inputInvoices));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }

        // GET: api/InputInvoice/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "admin, laboratory")]
        public async Task<ActionResult<InputInvoice>> GetInputInvoice(int id)
        {
            try
            {
                var inputInvoice = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);
                if (inputInvoice == null)
                {
                    return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
                }

                return Ok(_mapper.Map<InputInvoiceDTO>(inputInvoice));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }

        // PUT: api/InputInvoice/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "admin, laboratory")]
        public async Task<IActionResult> PutInputInvoice(int id, InputInvoiceUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);
                
                inputInvoiceDb.UpdateFromRequest(request);
                
                inputInvoiceDb.ModifiedAt = DateTime.UtcNow;
                inputInvoiceDb.ModifiedById = (int)HttpContext.Session.GetInt32("EmployeeId");

                var updatedInputInvoice = await _inputInvoiceService.UpdateInputInvoiceAsync(inputInvoiceDb);

                if (updatedInputInvoice == null)
                {
                    return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
                }

                return Ok(_mapper.Map<InputInvoiceDTO>(updatedInputInvoice));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
        
        
        // DELETE: api/InputInvoice/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteInputInvoice(int id)
        {
            try
            {
                var success = await _inputInvoiceService.DeleteInputInvoiceAsync(id);
                if (!success)
                {
                    return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
        
        // Patch: api/InputInvoice/5
        [HttpPatch("{id}/soft-remove")]
        //[Authorize(Roles = "admin, laboratory")]
        public async Task<IActionResult> SoftDeleteInputInvoice(int id)
        {
            try
            {
                var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);
                
                inputInvoiceDb.Removed = true;
                inputInvoiceDb.RemovedAt = DateTime.UtcNow;
                inputInvoiceDb.RemovedById = (int)HttpContext.Session.GetInt32("EmployeeId");
                
                var removedInputInvoice = await _inputInvoiceService.UpdateInputInvoiceAsync(inputInvoiceDb);
                
                if (removedInputInvoice == null)
                {
                    return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
                }

                return Ok(_mapper.Map<InputInvoiceDTO>(removedInputInvoice));
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
        
        // Patch: api/InputInvoice/5
        [HttpPatch("{id}/restore")]
        //[Authorize(Roles = "admin, laboratory")]
        public async Task<IActionResult> RestoreRemovedInputInvoice(int id)
        {
            try
            {
                var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);
                
                inputInvoiceDb.Removed = false;
                inputInvoiceDb.RestoredAt = DateTime.UtcNow;
                inputInvoiceDb.RestoreById = (int)HttpContext.Session.GetInt32("EmployeeId");
                
                var restorededInputInvoice = await _inputInvoiceService.UpdateInputInvoiceAsync(inputInvoiceDb);
                
                if (restorededInputInvoice == null)
                {
                    return NotFound($"Прибуткову накладну з ID {id} не знайдено.");
                }

                return Ok(_mapper.Map<InputInvoiceDTO>(restorededInputInvoice));
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
        
        
        [HttpGet("search")]
        //[Authorize(Roles = "admin, laboratory")]
        public ActionResult<IEnumerable<InputInvoiceDTO>> SearchInputInvoices(
            [FromQuery] int? id = null,
            [FromQuery] string invoiceNumber = null,
            [FromQuery] DateTime? arrivalDate = null,
            [FromQuery] string vehicleNumber = null,
            [FromQuery] int? supplierId = null,
            [FromQuery] int? productId = null,
            [FromQuery] int? createdById = null,
            [FromQuery] bool? removed = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            try
            {
                var query = _inputInvoiceService.GetInputInvoices(page, size).AsQueryable();

                if (id.HasValue)
                    query = query.Where(ii => ii.Id == id.Value);

                if (!string.IsNullOrEmpty(invoiceNumber))
                    query = query.Where(ii => ii.InvoiceNumber == invoiceNumber);

                if (arrivalDate.HasValue)
                    query = query.Where(ii => ii.ArrivalDate.Date == arrivalDate.Value.Date);

                if (!string.IsNullOrEmpty(vehicleNumber))
                    query = query.Where(ii => ii.VehicleNumber == vehicleNumber);

                if (supplierId.HasValue)
                    query = query.Where(ii => ii.SupplierId == supplierId.Value);

                if (productId.HasValue)
                    query = query.Where(ii => ii.ProductId == productId.Value);

                if (createdById.HasValue)
                    query = query.Where(ii => ii.CreatedById == createdById.Value);

                if (removed.HasValue)
                    query = query.Where(ii => ii.Removed == removed.Value);

                var result = query.ToList();
                return Ok(_mapper.Map<IEnumerable<InputInvoiceDTO>>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
        
        
        
    }

}
