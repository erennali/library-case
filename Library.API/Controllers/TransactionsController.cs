using Microsoft.AspNetCore.Mvc;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using FluentValidation;
using Library.Application.Validation;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IValidator<BorrowBookRequest> _borrowValidator;
    private readonly IValidator<ReturnBookRequest> _returnValidator;
    private readonly IValidator<RenewBookRequest> _renewValidator;

    public TransactionsController(
        ITransactionService transactionService,
        IValidator<BorrowBookRequest> borrowValidator,
        IValidator<ReturnBookRequest> returnValidator,
        IValidator<RenewBookRequest> renewValidator)
    {
        _transactionService = transactionService;
        _borrowValidator = borrowValidator;
        _returnValidator = returnValidator;
        _renewValidator = renewValidator;
    }

    [HttpPost("borrow")]
    public async Task<ActionResult<TransactionResponseDto>> BorrowBook([FromBody] BorrowBookRequest request, CancellationToken ct)
    {
        var validationResult = await _borrowValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _transactionService.BorrowBookAsync(request, ct);
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

    [HttpPost("return")]
    public async Task<ActionResult<TransactionResponseDto>> ReturnBook([FromBody] ReturnBookRequest request, CancellationToken ct)
    {
        var validationResult = await _returnValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _transactionService.ReturnBookAsync(request, ct);
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

    [HttpPost("renew")]
    public async Task<ActionResult<TransactionResponseDto>> RenewBook([FromBody] RenewBookRequest request, CancellationToken ct)
    {
        var validationResult = await _renewValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary()));

        try
        {
            var result = await _transactionService.RenewBookAsync(request, ct);
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
    public async Task<ActionResult<TransactionResponseDto>> GetById(int id, CancellationToken ct)
    {
        var transaction = await _transactionService.GetByIdAsync(id, ct);
        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpGet("member/{memberId:int}")]
    public async Task<ActionResult<PagedResult<TransactionResponseDto>>> GetByMember(
        int memberId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _transactionService.GetByMemberAsync(memberId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("book/{bookId:int}")]
    public async Task<ActionResult<PagedResult<TransactionResponseDto>>> GetByBook(
        int bookId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _transactionService.GetByBookAsync(bookId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<PagedResult<TransactionResponseDto>>> GetOverdue(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _transactionService.GetOverdueAsync(page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<PagedResult<TransactionResponseDto>>> GetActive(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _transactionService.GetActiveAsync(page, pageSize, ct);
        return Ok(result);
    }
}

