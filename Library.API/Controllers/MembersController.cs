using Library.Application.Members.Commands;
using Library.Application.Members.DTOs;
using Library.Application.Members.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberResponseDto>> GetById(int id, CancellationToken ct)
    {
        var res = await _mediator.Send(new GetMemberByIdQuery(id), ct);
        if (res is null) return NotFound();
        return Ok(res);
    }

    public record SearchRequest(int Page = 1, int PageSize = 10, string? Query = null);

    [HttpGet]
    public async Task<ActionResult<object>> Search([FromQuery] SearchRequest req, CancellationToken ct)
    {
        var res = await _mediator.Send(new SearchMembersQuery(req.Page, req.PageSize, req.Query), ct);
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponseDto>> Create([FromBody] MemberCreateDto dto, CancellationToken ct)
    {
        var created = await _mediator.Send(new CreateMemberCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MemberResponseDto>> Update(int id, [FromBody] MemberUpdateDto dto, CancellationToken ct)
    {
        var updated = await _mediator.Send(new UpdateMemberCommand(id, dto), ct);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteMemberCommand(id), ct);
        return NoContent();
    }
}


