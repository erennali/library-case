using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly IValidator<UpdateSettingRequest> _updateValidator;

    public SettingsController(
        ISettingsService settingsService,
        IValidator<UpdateSettingRequest> updateValidator)
    {
        _settingsService = settingsService;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<SettingResponseDto>>> GetAll(CancellationToken ct)
    {
        try
        {
            var settings = await _settingsService.GetAllAsync(ct);
            return Ok(settings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<SettingResponseDto>> GetByKey(string key, CancellationToken ct)
    {
        try
        {
            var setting = await _settingsService.GetByKeyAsync(key, ct);
            if (setting == null)
                return NotFound();

            return Ok(setting);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPut("{key}")]
    public async Task<ActionResult<SettingResponseDto>> Update(string key, [FromBody] UpdateSettingRequest request, CancellationToken ct)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _settingsService.UpdateAsync(key, request, ct);
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

    [HttpPost("bulk")]
    public async Task<ActionResult<List<SettingResponseDto>>> UpdateBulk([FromBody] UpdateBulkSettingsRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _settingsService.UpdateBulkAsync(request, ct);
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

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key, CancellationToken ct)
    {
        try
        {
            await _settingsService.DeleteAsync(key, ct);
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

    [HttpGet("library")]
    public async Task<ActionResult<LibrarySettingsDto>> GetLibrarySettings(CancellationToken ct)
    {
        try
        {
            var settings = await _settingsService.GetLibrarySettingsAsync(ct);
            return Ok(settings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPut("library")]
    public async Task<ActionResult<LibrarySettingsDto>> UpdateLibrarySettings([FromBody] UpdateLibrarySettingsRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _settingsService.UpdateLibrarySettingsAsync(request, ct);
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

    [HttpGet("fines")]
    public async Task<ActionResult<FineSettingsDto>> GetFineSettings(CancellationToken ct)
    {
        try
        {
            var settings = await _settingsService.GetFineSettingsAsync(ct);
            return Ok(settings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPut("fines")]
    public async Task<ActionResult<FineSettingsDto>> UpdateFineSettings([FromBody] UpdateFineSettingsRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _settingsService.UpdateFineSettingsAsync(request, ct);
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

    [HttpGet("notifications")]
    public async Task<ActionResult<NotificationSettingsDto>> GetNotificationSettings(CancellationToken ct)
    {
        try
        {
            var settings = await _settingsService.GetNotificationSettingsAsync(ct);
            return Ok(settings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPut("notifications")]
    public async Task<ActionResult<NotificationSettingsDto>> UpdateNotificationSettings([FromBody] UpdateNotificationSettingsRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _settingsService.UpdateNotificationSettingsAsync(request, ct);
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

    [HttpPost("reset/{key}")]
    public async Task<ActionResult<SettingResponseDto>> ResetToDefault(string key, CancellationToken ct)
    {
        try
        {
            var result = await _settingsService.ResetToDefaultAsync(key, ct);
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

    [HttpPost("reset-all")]
    public async Task<IActionResult> ResetAllToDefaults(CancellationToken ct)
    {
        try
        {
            await _settingsService.ResetAllToDefaultsAsync(ct);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}

