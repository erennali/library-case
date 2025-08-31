using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IValidator<CreateNotificationRequest> _createValidator;
    private readonly IValidator<SendBulkNotificationRequest> _bulkValidator;

    public NotificationsController(
        INotificationService notificationService,
        IValidator<CreateNotificationRequest> createValidator,
        IValidator<SendBulkNotificationRequest> bulkValidator)
    {
        _notificationService = notificationService;
        _createValidator = createValidator;
        _bulkValidator = bulkValidator;
    }

    [HttpPost]
    public async Task<ActionResult<NotificationResponseDto>> Create([FromBody] CreateNotificationRequest request, CancellationToken ct)
    {
        var validationResult = await _createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _notificationService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
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

    [HttpPost("bulk")]
    public async Task<ActionResult<BulkNotificationResponseDto>> SendBulk([FromBody] SendBulkNotificationRequest request, CancellationToken ct)
    {
        var validationResult = await _bulkValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _notificationService.SendBulkAsync(request, ct);
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NotificationResponseDto>> GetById(int id, CancellationToken ct)
    {
        var notification = await _notificationService.GetByIdAsync(id, ct);
        if (notification == null)
            return NotFound();

        return Ok(notification);
    }

    [HttpGet("member/{memberId:int}")]
    public async Task<ActionResult<PagedResult<NotificationResponseDto>>> GetByMember(
        int memberId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _notificationService.GetByMemberAsync(memberId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("unread/member/{memberId:int}")]
    public async Task<ActionResult<PagedResult<NotificationResponseDto>>> GetUnreadByMember(
        int memberId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _notificationService.GetUnreadByMemberAsync(memberId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<PagedResult<NotificationResponseDto>>> GetByType(
        string type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _notificationService.GetByTypeAsync(type, page, pageSize, ct);
        return Ok(result);
    }

    [HttpPost("mark-read/{id:int}")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken ct)
    {
        try
        {
            await _notificationService.MarkAsReadAsync(id, ct);
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

    [HttpPost("mark-read-bulk")]
    public async Task<IActionResult> MarkMultipleAsRead([FromBody] MarkNotificationsReadRequest request, CancellationToken ct)
    {
        try
        {
            await _notificationService.MarkMultipleAsReadAsync(request, ct);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await _notificationService.DeleteAsync(id, ct);
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

    [HttpGet("stats/member/{memberId:int}")]
    public async Task<ActionResult<NotificationStatsDto>> GetMemberStats(int memberId, CancellationToken ct)
    {
        var stats = await _notificationService.GetMemberStatsAsync(memberId, ct);
        return Ok(stats);
    }

    [HttpGet("overdue-reminders")]
    public async Task<ActionResult<PagedResult<NotificationResponseDto>>> GetOverdueReminders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _notificationService.GetOverdueRemindersAsync(page, pageSize, ct);
        return Ok(result);
    }
}

