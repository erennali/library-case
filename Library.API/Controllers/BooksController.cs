using AutoMapper;
using Library.Application.Books.Commands;
using Library.Application.Books.DTOs;
using Library.Application.Books.Queries;
using Library.Application.Books.Validation;
using Library.Application.Abstractions.Services;
using Library.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BooksController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> GetById(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new GetBookByIdQuery(id), ct);
        if (response is null) return NotFound();
        return Ok(response);
    }

    public record SearchRequest(int Page = 1, int PageSize = 10, string? Query = null, int? CategoryId = null);

    [HttpGet]
    public async Task<ActionResult<object>> Search([FromQuery] SearchRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new SearchBooksQuery(req.Page, req.PageSize, req.Query, req.CategoryId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Create([FromBody] BookCreateDto dto, CancellationToken ct)
    {
        var validator = new BookCreateDtoValidator();
        var result = await validator.ValidateAsync(dto, ct);
        if (!result.IsValid) return BadRequest(new ValidationProblemDetails(result.ToDictionary()));

        var response = await _mediator.Send(new CreateBookCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> Update(int id, [FromBody] BookUpdateDto dto, CancellationToken ct)
    {
        var validator = new BookUpdateDtoValidator();
        var result = await validator.ValidateAsync(dto, ct);
        if (!result.IsValid) return BadRequest(new ValidationProblemDetails(result.ToDictionary()));

        var response = await _mediator.Send(new UpdateBookCommand(id, dto), ct);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteBookCommand(id), ct);
        return NoContent();
    }
}


