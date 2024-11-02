using AutoMapper;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[Route("api/completion-report-operation")]
[ApiController]
public class CompletionReportOperationController : ControllerBase

{
    private readonly ICompletionReportOperationService _completionReportOperationService;
    private readonly IMapper _mapper;
    private readonly ILogger<CompletionReportOperationController> _logger;

    public CompletionReportOperationController(ICompletionReportOperationService completionReportOperationService, IMapper mapper, ILogger<CompletionReportOperationController> logger)
    {
        _completionReportOperationService = completionReportOperationService;
        _mapper = mapper;
        _logger = logger;
    }

    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }

    [HttpPost]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<CompletionReportOperationDto>> CreateCompletionReportOperation(CompletionReportOperationCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var cancellationToken = GetCancellationToken();
            var newCompletionReportOperation = _mapper.Map<CompletionReportOperation>(request);
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();

            var createdCompletionReportOperation =
                await _completionReportOperationService.CreateCompletionReportOperationAsync(newCompletionReportOperation, createdById, cancellationToken);
            return CreatedAtAction(nameof(GetCompletionReportOperation), new { id = createdCompletionReportOperation.Id },
                _mapper.Map<CompletionReportOperationDto>(createdCompletionReportOperation));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Технологичної операції акта виконаних робіт: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Технологичної операції акта виконаних робіт: {ex.Message}");
        }
    }


    [HttpGet]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<IEnumerable<CompletionReportOperationDto>>> GetCompletionReportOperation([FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReportOperations = await _completionReportOperationService.GetCompletionReportOperations(page, size, cancellationToken);

            var completionReportOperationDtos = _mapper.Map<IEnumerable<CompletionReportOperationDto>>(completionReportOperations);
            return Ok(completionReportOperationDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Технологичної операції акта виконаних робіт: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Технологичної операції акта виконаних робіт: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<CompletionReportOperationDto>> GetCompletionReportOperation(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReportOperation = await _completionReportOperationService.GetCompletionReportOperationByIdAsync(id, cancellationToken);
            if (completionReportOperation == null)
            {
                return NotFound($"Технологичної операції акта виконаних робіт з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<CompletionReportOperationDto>(completionReportOperation));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }


    [HttpGet("search")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<ActionResult<IEnumerable<CompletionReportOperationDto>>> SearchCompletionReportOperations(
        [FromQuery] int? id = null,
        [FromQuery] int? technologicalOperationId = null,
        [FromQuery] double? amount = null,
        [FromQuery] int? completionReportId = null,
        [FromQuery] int? createdById = null,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            // передаємо параметри у сервіс для фільтрації
            var filteredCompletionReportOperations = await _completionReportOperationService.SearchCompletionReportOperations(
                id,
                technologicalOperationId,
                amount,
                completionReportId,
                createdById,
                page,
                size,
                cancellationToken);

            var CompletionReportOperationDtos = _mapper.Map<IEnumerable<CompletionReportOperationDto>>(filteredCompletionReportOperations);
            return Ok(CompletionReportOperationDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Технологичної операції акта виконаних робіт за параметрами: {ex.Message}");
            return StatusCode(500,
                $"Внутрішня помилка сервера при отриманні Технологичної операції акта виконаних робіт за параметрами: {ex.Message}");
        }
    }


    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> UpdateCompletionReportOperation(int id, CompletionReportOperationUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReportOperationDb = await _completionReportOperationService.GetCompletionReportOperationByIdAsync(id, cancellationToken);
            if (completionReportOperationDb == null)
            {
                return NotFound($"Технологичної операції акта виконаних робіт з ID {id} не знайдено.");
            }

            completionReportOperationDb.UpdateFromRequest(request);
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedCompletionReportOperation =
                await _completionReportOperationService.UpdateCompletionReportOperationAsync(completionReportOperationDb, modifiedById, cancellationToken);

            return Ok(_mapper.Map<CompletionReportOperationDto>(updatedCompletionReportOperation));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при при оновленні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }


    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> SoftDeleteLaboratoryCar(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReportOperationDb = await _completionReportOperationService.GetCompletionReportOperationByIdAsync(id, cancellationToken);
            if (completionReportOperationDb == null)
            {
                return NotFound($"Технологичної операції акта виконаних робіт з ID {id} не знайдено.");
            }

            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedCompletionReportOperation =
                await _completionReportOperationService.SoftDeleteCompletionReportOperationAsync(completionReportOperationDb, removedById, cancellationToken);

            return Ok(_mapper.Map<CompletionReportOperationDto>(removedCompletionReportOperation));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }


    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> RestoreRemovedCompletionReportOperation(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var completionReportOperationDb = await _completionReportOperationService.GetCompletionReportOperationByIdAsync(id, cancellationToken);
            if (completionReportOperationDb == null)
            {
                return NotFound($"Технологичної операції акта виконаних робіт з ID {id} не знайдено.");
            }

            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredCompletionReportOperation =
                await _completionReportOperationService.RestoreRemovedCompletionReportOperationAsync(completionReportOperationDb, restoredById, cancellationToken);

            return Ok(_mapper.Map<CompletionReportOperationDto>(restoredCompletionReportOperation));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }


    [HttpDelete("{id}/hard-remove")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCompletionReportOperation(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _completionReportOperationService.DeleteCompletionReportOperationAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound($"Технологичної операції акта виконаних робіт з ID {id} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Технологичної операції акта виконаних робіт з ID {id}: {ex.Message}");
        }
    }
}