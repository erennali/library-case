using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportExportController : ControllerBase
{
    private readonly IImportExportService _importExportService;
    private readonly IValidator<ImportRequest> _importValidator;
    private readonly IValidator<ExportRequest> _exportValidator;

    public ImportExportController(
        IImportExportService importExportService,
        IValidator<ImportRequest> importValidator,
        IValidator<ExportRequest> exportValidator)
    {
        _importExportService = importExportService;
        _importValidator = importValidator;
        _exportValidator = exportValidator;
    }

    [HttpPost("import")]
    public async Task<ActionResult<ImportResultDto>> Import([FromForm] ImportRequest request, CancellationToken ct)
    {
        var validationResult = await _importValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _importExportService.ImportAsync(request, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("export")]
    public async Task<IActionResult> Export([FromBody] ExportRequest request, CancellationToken ct)
    {
        var validationResult = await _exportValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _importExportService.ExportAsync(request, ct);

            if (result == null)
                return NotFound();

            return File(result.FileBytes, result.ContentType, result.FileName);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("templates")]
    public async Task<ActionResult<List<ExportTemplateDto>>> GetExportTemplates(CancellationToken ct)
    {
        try
        {
            var templates = await _importExportService.GetExportTemplatesAsync(ct);
            return Ok(templates);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("templates/{id:int}/download")]
    public async Task<IActionResult> DownloadTemplate(int id, CancellationToken ct)
    {
        try
        {
            var template = await _importExportService.DownloadTemplateAsync(id, ct);

            if (template == null)
                return NotFound();

            return File(template.FileBytes, template.ContentType, template.FileName);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("import-history")]
    public async Task<ActionResult<PagedResult<ImportHistoryDto>>> GetImportHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var history = await _importExportService.GetImportHistoryAsync(page, pageSize, status, fromDate, toDate, ct);
            return Ok(history);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("export-history")]
    public async Task<ActionResult<PagedResult<ExportHistoryDto>>> GetExportHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? type = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var history = await _importExportService.GetExportHistoryAsync(page, pageSize, type, fromDate, toDate, ct);
            return Ok(history);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("import/{id:int}")]
    public async Task<ActionResult<ImportDetailDto>> GetImportDetail(int id, CancellationToken ct)
    {
        try
        {
            var detail = await _importExportService.GetImportDetailAsync(id, ct);
            if (detail == null)
                return NotFound();

            return Ok(detail);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("export/{id:int}")]
    public async Task<ActionResult<ExportDetailDto>> GetExportDetail(int id, CancellationToken ct)
    {
        try
        {
            var detail = await _importExportService.GetExportDetailAsync(id, ct);
            if (detail == null)
                return NotFound();

            return Ok(detail);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("import/{id:int}/cancel")]
    public async Task<IActionResult> CancelImport(int id, CancellationToken ct)
    {
        try
        {
            await _importExportService.CancelImportAsync(id, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("import/{id:int}/retry")]
    public async Task<ActionResult<ImportResultDto>> RetryImport(int id, CancellationToken ct)
    {
        try
        {
            var result = await _importExportService.RetryImportAsync(id, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("supported-formats")]
    public async Task<ActionResult<SupportedFormatsDto>> GetSupportedFormats(CancellationToken ct)
    {
        try
        {
            var formats = await _importExportService.GetSupportedFormatsAsync(ct);
            return Ok(formats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("validation-rules")]
    public async Task<ActionResult<List<ValidationRuleDto>>> GetValidationRules(CancellationToken ct)
    {
        try
        {
            var rules = await _importExportService.GetValidationRulesAsync(ct);
            return Ok(rules);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("validate")]
    public async Task<ActionResult<ValidationResultDto>> ValidateData([FromForm] ValidateDataRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _importExportService.ValidateDataAsync(request, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("schedule-export")]
    public async Task<ActionResult<ScheduledExportDto>> ScheduleExport([FromBody] ScheduleExportRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _importExportService.ScheduleExportAsync(request, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("scheduled-exports")]
    public async Task<ActionResult<List<ScheduledExportDto>>> GetScheduledExports(CancellationToken ct)
    {
        try
        {
            var exports = await _importExportService.GetScheduledExportsAsync(ct);
            return Ok(exports);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpDelete("scheduled-exports/{id:int}")]
    public async Task<IActionResult> CancelScheduledExport(int id, CancellationToken ct)
    {
        try
        {
            await _importExportService.CancelScheduledExportAsync(id, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}

