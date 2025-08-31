using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinesController : ControllerBase
{
    private readonly IFineService _fineService;
    private readonly IValidator<PayFineRequest> _payFineValidator;
    private readonly IValidator<WaiveFineRequest> _waiveFineValidator;

    public FinesController(
        IFineService fineService,
        IValidator<PayFineRequest> payFineValidator,
        IValidator<WaiveFineRequest> waiveFineValidator)
    {
        _fineService = fineService;
        _payFineValidator = payFineValidator;
        _waiveFineValidator = waiveFineValidator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FineResponseDto>> GetById(int id, CancellationToken ct)
    {
        var fine = await _fineService.GetByIdAsync(id, ct);
        if (fine == null)
            return NotFound();

        return Ok(fine);
    }

    [HttpGet("member/{memberId:int}")]
    public async Task<ActionResult<PagedResult<FineResponseDto>>> GetByMember(
        int memberId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _fineService.GetByMemberAsync(memberId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<PagedResult<FineResponseDto>>> GetPending(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _fineService.GetPendingAsync(page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<PagedResult<FineResponseDto>>> GetOverdue(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _fineService.GetOverdueAsync(page, pageSize, ct);
        return Ok(result);
    }

    [HttpPost("pay")]
    public async Task<ActionResult<FineResponseDto>> PayFine([FromBody] PayFineRequest request, CancellationToken ct)
    {
        var validationResult = await _payFineValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _fineService.PayFineAsync(request, ct);
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

    [HttpPost("waive")]
    public async Task<ActionResult<FineResponseDto>> WaiveFine([FromBody] WaiveFineRequest request, CancellationToken ct)
    {
        var validationResult = await _waiveFineValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _fineService.WaiveFineAsync(request, ct);
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

    [HttpGet("summary/member/{memberId:int}")]
    public async Task<ActionResult<FineSummaryDto>> GetMemberSummary(int memberId, CancellationToken ct)
    {
        var summary = await _fineService.GetMemberSummaryAsync(memberId, ct);
        return Ok(summary);
    }

    [HttpGet("summary/overall")]
    public async Task<ActionResult<OverallFineSummaryDto>> GetOverallSummary(CancellationToken ct)
    {
        var summary = await _fineService.GetOverallSummaryAsync(ct);
        return Ok(summary);
    }
}

