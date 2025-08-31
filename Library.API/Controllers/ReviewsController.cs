using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IValidator<CreateReviewRequest> _createValidator;
    private readonly IValidator<UpdateReviewRequest> _updateValidator;

    public ReviewsController(
        IReviewService reviewService,
        IValidator<CreateReviewRequest> createValidator,
        IValidator<UpdateReviewRequest> updateValidator)
    {
        _reviewService = reviewService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ReviewResponseDto>> Create([FromBody] CreateReviewRequest request, CancellationToken ct)
    {
        var validationResult = await _createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _reviewService.CreateAsync(request, ct);
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
    public async Task<ActionResult<ReviewResponseDto>> Update(int id, [FromBody] UpdateReviewRequest request, CancellationToken ct)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _reviewService.UpdateAsync(id, request, ct);
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
            await _reviewService.DeleteAsync(id, ct);
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
    public async Task<ActionResult<ReviewResponseDto>> GetById(int id, CancellationToken ct)
    {
        var review = await _reviewService.GetByIdAsync(id, ct);
        if (review == null)
            return NotFound();

        return Ok(review);
    }

    [HttpGet("book/{bookId:int}")]
    public async Task<ActionResult<PagedResult<ReviewResponseDto>>> GetByBook(
        int bookId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _reviewService.GetByBookAsync(bookId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("member/{memberId:int}")]
    public async Task<ActionResult<PagedResult<ReviewResponseDto>>> GetByMember(
        int memberId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _reviewService.GetByMemberAsync(memberId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("approved")]
    public async Task<ActionResult<PagedResult<ReviewResponseDto>>> GetApproved(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _reviewService.GetApprovedAsync(page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<PagedResult<ReviewResponseDto>>> GetPending(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _reviewService.GetPendingAsync(page, pageSize, ct);
        return Ok(result);
    }

    [HttpPost("approve/{id:int}")]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
    {
        try
        {
            await _reviewService.ApproveAsync(id, ct);
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

    [HttpPost("reject/{id:int}")]
    public async Task<IActionResult> Reject(int id, [FromBody] RejectReviewRequest request, CancellationToken ct)
    {
        try
        {
            await _reviewService.RejectAsync(id, request, ct);
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

    [HttpGet("stats/book/{bookId:int}")]
    public async Task<ActionResult<BookReviewStatsDto>> GetBookStats(int bookId, CancellationToken ct)
    {
        var stats = await _reviewService.GetBookStatsAsync(bookId, ct);
        return Ok(stats);
    }
}

