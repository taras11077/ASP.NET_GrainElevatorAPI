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


        // GET: api/InputInvoice/search?invoiceNumber=123456
        [HttpGet("search")]
        //[Authorize(Roles = "admin, laboratory")]
        public ActionResult<IEnumerable<InputInvoice>> SearchInputInvoices(string invoiceNumber)
        {
            try
            {
                var inputInvoices = _inputInvoiceService.SearchInputInvoice(invoiceNumber);
                return Ok(_mapper.Map<IEnumerable<InputInvoiceDTO>>(inputInvoices));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
    }

}
