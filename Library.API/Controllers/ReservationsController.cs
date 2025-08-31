using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IValidator<CreateReservationRequest> _createValidator;
    private readonly IValidator<CancelReservationRequest> _cancelValidator;

    public ReservationsController(
        IReservationService reservationService,
        IValidator<CreateReservationRequest> createValidator,
        IValidator<CancelReservationRequest> cancelValidator)
    {
        _reservationService = reservationService;
        _createValidator = createValidator;
        _cancelValidator = cancelValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> Create([FromBody] CreateReservationRequest request, CancellationToken ct)
    {
        var validationResult = await _createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _reservationService.CreateAsync(request, ct);
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

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelReservationRequest request, CancellationToken ct)
    {
        var validationResult = await _cancelValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            await _reservationService.CancelAsync(request, ct);
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReservationResponseDto>> GetById(int id, CancellationToken ct)
    {
        var reservation = await _reservationService.GetByIdAsync(id, ct);
        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpGet("member/{memberId:int}")]
    public async Task<ActionResult<PagedResult<ReservationResponseDto>>> GetByMember(
        int memberId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _reservationService.GetByMemberAsync(memberId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("book/{bookId:int}")]
    public async Task<ActionResult<PagedResult<ReservationResponseDto>>> GetByBook(
        int bookId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _reservationService.GetByBookAsync(bookId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<PagedResult<ReservationResponseDto>>> GetActive(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _reservationService.GetActiveAsync(page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("expired")]
    public async Task<ActionResult<PagedResult<ReservationResponseDto>>> GetExpired(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _reservationService.GetExpiredAsync(page, pageSize, ct);
        return Ok(result);
    }

    [HttpPost("fulfill/{id:int}")]
    public async Task<IActionResult> Fulfill(int id, CancellationToken ct)
    {
        try
        {
            await _reservationService.FulfillAsync(id, ct);
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

