using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[Route("api/completion-report")]
[ApiController]
public class CompletionReportController: ControllerBase
{
    private readonly ICompletionReportService _completionReportService;
    private readonly IMapper _mapper;
    private readonly ILogger<CompletionReportController> _logger;

    public CompletionReportController(ICompletionReportService completionReportService, IMapper mapper, ILogger<CompletionReportController> logger)
    {
        _completionReportService = completionReportService;
        _mapper = mapper;
        _logger = logger;
    }
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    
    [HttpPost]
    //[Authorize(Roles = "Admin, Technologist")]
    public async Task<ActionResult<CompletionReportDto>> CreateCompletionReport([FromBody] CompletionReportCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            // створення Акта виконаних робіт
            var createdCompletionReport = await _completionReportService.CreateCompletionReportAsync(
                request.ReportNumber,
                request.RegisterIds,
                request.OperationIds,
                createdById,
                cancellationToken);
            
            return CreatedAtAction(nameof(GetCompletionReports), new { id = createdCompletionReport.Id },
                _mapper.Map<CompletionReportDto>(createdCompletionReport));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Акта виконаних робіт: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Акта виконаних робіт: {ex.Message}");
        }
    }
    
    
    [HttpPut("{id}/cost-calculate")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult>CalculateCostCompletionReport(int id, int priceListId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();

            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var calculatedCompletionReport = await _completionReportService.CalculateReportCostAsync(id, priceListId, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<CompletionReportDto>(calculatedCompletionReport));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при при обчисленні вартості Акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при обчисленні вартості Акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpGet]
    //[Authorize(Roles = "Admin, Technologist, Accountant ")]
    public async Task<ActionResult<IEnumerable<CompletionReportDto>>> GetCompletionReports([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReports = await _completionReportService.GetCompletionReports(page, size, cancellationToken);

            var completionReportDtos = _mapper.Map<IEnumerable<CompletionReportDto>>(completionReports);
            return Ok(completionReportDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Актів виконаних робіт: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Актів виконаних робіт: {ex.Message}");
        }
    }
    
    
    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin, Technologist, Accountant")]
    public async Task<ActionResult<CompletionReportDto>> GetCompletionReportById(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var completionReport = await _completionReportService.GetCompletionReportByIdAsync(id, cancellationToken);
            if (completionReport == null)
            {
                return NotFound($"Акта виконаних робіт з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<CompletionReportDto>(completionReport));
        }
        catch (Exception ex)
        {
            
            _logger.LogError($"Внутрішня помилка сервера при отриманні Акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }
    
    [HttpGet("search")]
    //[Authorize(Roles = "Admin, Technologist, Accountant")]
    public async Task<ActionResult<IEnumerable<CompletionReportDto>>> SearchCompletionReports(
        [FromQuery] int? id,
        [FromQuery] string? reportNumber,
        [FromQuery] DateTime? reportDate,
        [FromQuery] int? quantitiesDryingReport,
        [FromQuery] int? physicalWeightReport,
        [FromQuery] int? supplierId,
        [FromQuery] int? productId,
        [FromQuery] int? createdById,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();

            var filteredCompletionReports = await _completionReportService.SearchCompletionReports(
                id, 
                reportNumber,
                reportDate,
                quantitiesDryingReport,
                physicalWeightReport,
                supplierId,
                productId,
                createdById, 
                page, 
                size,
                cancellationToken);

            var completionReportDtos = _mapper.Map<IEnumerable<CompletionReportDto>>(filteredCompletionReports);
            return Ok(completionReportDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Актів виконаних робіт за параметрами: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Актів виконаних робіт за параметрами: {ex.Message}");
        }
    }
    
    
    
    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> UpdateCompletionReport(int id, CompletionReportUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReportDb = await _completionReportService.GetCompletionReportByIdAsync(id, cancellationToken);
            if (completionReportDb == null)
            {
                return NotFound($"Акта виконаних робіт з ID {id} не знайдено.");
            }
            
            completionReportDb.UpdateFromRequest(request);
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedCompletionReport = await _completionReportService.UpdateCompletionReportAsync(completionReportDb, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<CompletionReportDto>(updatedCompletionReport));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при при оновленні Акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "Admin, Technologist, Accountant")]
    public async Task<IActionResult> SoftDeleteCompletionReport(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReportDb = await _completionReportService.GetCompletionReportByIdAsync(id, cancellationToken);
            if (completionReportDb == null)
            {
                return NotFound($"Акта виконаних робіт з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedCompletionReport = await _completionReportService.SoftDeleteCompletionReportAsync(completionReportDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<CompletionReportDto>(removedCompletionReport));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "Admin, Technologist, Accountant")]
    public async Task<IActionResult> RestoreRemovedCompletionReport(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReportDb = await _completionReportService.GetCompletionReportByIdAsync(id, cancellationToken);
            if (completionReportDb == null)
            {
                return NotFound($"Акта виконаних робіт з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredCompletionReport = await _completionReportService.RestoreRemovedCompletionReportAsync(completionReportDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<CompletionReportDto>(restoredCompletionReport));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCompletionReport(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            
            var success = await _completionReportService.DeleteCompletionReportAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Акта виконаних робіт з ID {id} не знайдено.");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }
}