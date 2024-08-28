using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;
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
        public async Task<ActionResult<InputInvoice>> PostInputInvoice(InputInvoiceCreateRequest request)
        {
            try
            {
                var newInputInvoice = _mapper.Map<InputInvoice>(request);
                
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
        public async Task<IActionResult> PutInputInvoice(int id, InputInvoiceCreateRequest request)
        {
            try
            {
                var inputInvoiceDb = await _inputInvoiceService.GetInputInvoiceByIdAsync(id);

                //productDb.Title = request.Title;

                var updatedInputInvoice = await _inputInvoiceService.UpdateInputInvoiceAsync(inputInvoiceDb);

                if (updatedInputInvoice == null)
                {
                    return NotFound($"Прибуткову накладну з ID {id} не знайдений.");
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
        public async Task<IActionResult> DeleteInputInvoice(int id)
        {
            try
            {
                var success = await _inputInvoiceService.DeleteInputInvoiceAsync(id);
                if (!success)
                {
                    return NotFound($"Прибуткову накладну з ID {{id}} не знайдено.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }


        // GET: api/InputInvoice/search?invoiceNumber=123456
        [HttpGet("search")]
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
