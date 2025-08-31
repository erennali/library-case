using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrariansController : ControllerBase
{
    private readonly ILibrarianService _librarianService;
    private readonly IValidator<CreateLibrarianRequest> _createValidator;
    private readonly IValidator<UpdateLibrarianRequest> _updateValidator;

    public LibrariansController(
        ILibrarianService librarianService,
        IValidator<CreateLibrarianRequest> createValidator,
        IValidator<UpdateLibrarianRequest> updateValidator)
    {
        _librarianService = librarianService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibrarianResponseDto>> GetById(int id, CancellationToken ct)
    {
        var librarian = await _librarianService.GetByIdAsync(id, ct);
        if (librarian == null)
            return NotFound();

        return Ok(librarian);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<LibrarianResponseDto>>> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? query = null,
        [FromQuery] string? role = null,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        var result = await _librarianService.GetListAsync(page, pageSize, query, role, status, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<LibrarianResponseDto>> Create([FromBody] CreateLibrarianRequest request, CancellationToken ct)
    {
        var validationResult = await _createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _librarianService.CreateAsync(request, ct);
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

    [HttpPut("{id:int}")]
    public async Task<ActionResult<LibrarianResponseDto>> Update(int id, [FromBody] UpdateLibrarianRequest request, CancellationToken ct)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _librarianService.UpdateAsync(id, request, ct);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await _librarianService.DeleteAsync(id, ct);
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

    [HttpPost("activate/{id:int}")]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        try
        {
            await _librarianService.ActivateAsync(id, ct);
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

    [HttpPost("deactivate/{id:int}")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
    {
        try
        {
            await _librarianService.DeactivateAsync(id, ct);
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

    [HttpPost("change-role/{id:int}")]
    public async Task<ActionResult<LibrarianResponseDto>> ChangeRole(int id, [FromBody] ChangeLibrarianRoleRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _librarianService.ChangeRoleAsync(id, request, ct);
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

    [HttpGet("stats")]
    public async Task<ActionResult<LibrarianStatsDto>> GetStats(CancellationToken ct)
    {
        try
        {
            var stats = await _librarianService.GetStatsAsync(ct);
            return Ok(stats);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("activity/{id:int}")]
    public async Task<ActionResult<PagedResult<LibrarianActivityDto>>> GetActivity(
        int id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var activity = await _librarianService.GetActivityAsync(id, page, pageSize, fromDate, toDate, ct);
            return Ok(activity);
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

