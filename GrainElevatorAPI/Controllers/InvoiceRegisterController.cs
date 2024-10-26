using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace GrainElevatorAPI.Controllers;

[Route("api/register")]
[ApiController]
public class InvoiceRegisterController : ControllerBase
{
    private readonly IInvoiceRegisterService _invoiceRegisterService;
    private readonly IMapper _mapper;

    public InvoiceRegisterController(IInvoiceRegisterService invoiceRegisterService, IMapper mapper)
    {
        _invoiceRegisterService = invoiceRegisterService;
        _mapper = mapper;
    }
    
    
    [HttpPost]
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<ActionResult<Register>> CreateRegister([FromBody] InvoiceRegisterCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        try
        {

            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
        
            var createdRegister = await _invoiceRegisterService.CreateRegisterAsync(
                request.SupplierId,
                request.ProductId,
                request.ArrivalDate,
                request.WeedImpurityBase,
                request.MoistureBase,
                request.LaboratoryCardIds,
                createdById);
            
            return CreatedAtAction(nameof(GetRegisters), new { id = createdRegister.Id },
                _mapper.Map<InvoiceRegisterDTO>(createdRegister));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    
    [HttpGet]
    //[Authorize(Roles = "Admin, Technologist")]
    public ActionResult<IEnumerable<Register>> GetRegisters([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var registers = _invoiceRegisterService.GetRegisters(page, size);
            return Ok(_mapper.Map<IEnumerable<InvoiceRegisterDTO>>(registers));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    
    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<ActionResult<Register>> GetRegisterById(int id)
    {
        try
        {
            var register = await _invoiceRegisterService.GetRegisterByIdAsync(id);
            if (register == null)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<InvoiceRegisterDTO>(register));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRegister(int id)
    {
        try
        {
            var success = await _invoiceRegisterService.DeleteRegisterAsync(id);
            if (!success)
            {
                return NotFound($"Реєстру з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
}
    
    
    
    

