using AutoMapper;
using Library.Application.Abstractions.Services;
using Library.Application.Common.Pagination;
using Library.Application.Members.DTOs;
using MediatR;

namespace Library.Application.Members.Queries;

public record SearchMembersQuery(int Page, int PageSize, string? Query) : IRequest<PagedResult<MemberResponseDto>>;

public class SearchMembersHandler : IRequestHandler<SearchMembersQuery, PagedResult<MemberResponseDto>>
{
    private readonly IMemberService _service;
    private readonly IMapper _mapper;

    public SearchMembersHandler(IMemberService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<PagedResult<MemberResponseDto>> Handle(SearchMembersQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _service.SearchAsync(request.Page, request.PageSize, request.Query, cancellationToken);
        var mapped = items.Select(_mapper.Map<MemberResponseDto>).ToList();
        return new PagedResult<MemberResponseDto>(mapped, total, request.Page, request.PageSize);
    }
}


