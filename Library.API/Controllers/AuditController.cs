using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditController : ControllerBase
{
    private readonly IAuditService _auditService;

    public AuditController(IAuditService auditService)
    {
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AuditLogDto>>> GetAuditLogs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? entityType = null,
        [FromQuery] string? action = null,
        [FromQuery] int? userId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var logs = await _auditService.GetAuditLogsAsync(page, pageSize, entityType, action, userId, fromDate, toDate, ct);
            return Ok(logs);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuditLogDto>> GetById(int id, CancellationToken ct)
    {
        try
        {
            var log = await _auditService.GetByIdAsync(id, ct);
            if (log == null)
                return NotFound();

            return Ok(log);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("entity/{entityType}/{entityId:int}")]
    public async Task<ActionResult<PagedResult<AuditLogDto>>> GetByEntity(
        string entityType,
        int entityId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        try
        {
            var logs = await _auditService.GetByEntityAsync(entityType, entityId, page, pageSize, ct);
            return Ok(logs);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<PagedResult<AuditLogDto>>> GetByUser(
        int userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var logs = await _auditService.GetByUserAsync(userId, page, pageSize, fromDate, toDate, ct);
            return Ok(logs);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("actions")]
    public async Task<ActionResult<List<string>>> GetAvailableActions(CancellationToken ct)
    {
        try
        {
            var actions = await _auditService.GetAvailableActionsAsync(ct);
            return Ok(actions);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("entity-types")]
    public async Task<ActionResult<List<string>>> GetAvailableEntityTypes(CancellationToken ct)
    {
        try
        {
            var types = await _auditService.GetAvailableEntityTypesAsync(ct);
            return Ok(types);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("summary")]
    public async Task<ActionResult<AuditSummaryDto>> GetSummary(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var summary = await _auditService.GetSummaryAsync(fromDate, toDate, ct);
            return Ok(summary);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("changes/{id:int}")]
    public async Task<ActionResult<EntityChangesDto>> GetEntityChanges(int id, CancellationToken ct)
    {
        try
        {
            var changes = await _auditService.GetEntityChangesAsync(id, ct);
            if (changes == null)
                return NotFound();

            return Ok(changes);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportAuditLogs(
        [FromQuery] string format = "csv",
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? entityType = null,
        [FromQuery] string? action = null,
        [FromQuery] int? userId = null,
        CancellationToken ct = default)
    {
        try
        {
            var exportResult = await _auditService.ExportAuditLogsAsync(format, fromDate, toDate, entityType, action, userId, ct);

            if (exportResult == null)
                return NotFound();

            return File(exportResult.FileBytes, exportResult.ContentType, exportResult.FileName);
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

    [HttpPost("purge")]
    public async Task<IActionResult> PurgeOldLogs([FromBody] PurgeAuditLogsRequest request, CancellationToken ct)
    {
        try
        {
            await _auditService.PurgeOldLogsAsync(request, ct);
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

    [HttpGet("retention-policy")]
    public async Task<ActionResult<AuditRetentionPolicyDto>> GetRetentionPolicy(CancellationToken ct)
    {
        try
        {
            var policy = await _auditService.GetRetentionPolicyAsync(ct);
            return Ok(policy);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPut("retention-policy")]
    public async Task<ActionResult<AuditRetentionPolicyDto>> UpdateRetentionPolicy([FromBody] UpdateAuditRetentionPolicyRequest request, CancellationToken ct)
    {
        try
        {
            var policy = await _auditService.UpdateRetentionPolicyAsync(request, ct);
            return Ok(policy);
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

