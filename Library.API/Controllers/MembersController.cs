using Library.Application.Abstractions.Services;
using Library.Domain.Entities;
using Library.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Member>> GetById(int id, CancellationToken ct)
    {
        var member = await _memberService.GetAsync(id, ct);
        if (member == null)
            return NotFound();

        return Ok(member);
    }

    [HttpGet]
    public async Task<ActionResult<(IReadOnlyList<Member> Items, int TotalCount)>> Search([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, CancellationToken ct = default)
    {
        var result = await _memberService.SearchAsync(page, pageSize, search, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Member>> Create([FromBody] Member member, CancellationToken ct)
    {
        var createdMember = await _memberService.CreateAsync(member, ct);
        return CreatedAtAction(nameof(GetById), new { id = createdMember.Id }, createdMember);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Member updatedMember, CancellationToken ct)
    {
        await _memberService.UpdateAsync(id, updatedMember, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _memberService.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpPost("extend-membership/{id:int}")]
    public async Task<IActionResult> ExtendMembership(int id, [FromBody] ExtendMembershipRequest request, CancellationToken ct)
    {
        var member = await _memberService.GetAsync(id, ct);
        if (member == null)
            return NotFound();

        member.MembershipEndDate = request.NewEndDate;
        await _memberService.UpdateAsync(id, member, ct);
        return NoContent();
    }
}


