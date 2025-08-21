using FluentValidation;
using Library.Application.Categories.Commands;
using Library.Application.Categories.DTOs;
using Library.Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> GetById(int id, CancellationToken ct)
    {
        var res = await _mediator.Send(new GetCategoryByIdQuery(id), ct);
        if (res is null) return NotFound();
        return Ok(res);
    }

    public record SearchRequest(int Page = 1, int PageSize = 10, string? Query = null, int? ParentCategoryId = null);

    [HttpGet]
    public async Task<ActionResult<object>> Search([FromQuery] SearchRequest req, CancellationToken ct)
    {
        var res = await _mediator.Send(new SearchCategoriesQuery(req.Page, req.PageSize, req.Query, req.ParentCategoryId), ct);
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CategoryCreateDto dto, CancellationToken ct)
    {
        var created = await _mediator.Send(new CreateCategoryCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> Update(int id, [FromBody] CategoryUpdateDto dto, CancellationToken ct)
    {
        var updated = await _mediator.Send(new UpdateCategoryCommand(id, dto), ct);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCategoryCommand(id), ct);
        return NoContent();
    }
}


