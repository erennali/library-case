using AutoMapper;
using Library.Application.Abstractions.Services;
using Library.Application.Members.DTOs;
using MediatR;

namespace Library.Application.Members.Queries;

public record GetMemberByIdQuery(int Id) : IRequest<MemberResponseDto?>;

public class GetMemberByIdHandler : IRequestHandler<GetMemberByIdQuery, MemberResponseDto?>
{
    private readonly IMemberService _service;
    private readonly IMapper _mapper;

    public GetMemberByIdHandler(IMemberService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<MemberResponseDto?> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _service.GetAsync(request.Id, cancellationToken);
        return member is null ? null : _mapper.Map<MemberResponseDto>(member);
    }
}


