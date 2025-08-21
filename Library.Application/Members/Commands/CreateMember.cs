using AutoMapper;
using FluentValidation;
using Library.Application.Abstractions.Services;
using Library.Application.Members.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Members.Commands;

public record CreateMemberCommand(MemberCreateDto Dto) : IRequest<MemberResponseDto>;

public class CreateMemberHandler : IRequestHandler<CreateMemberCommand, MemberResponseDto>
{
    private readonly IMemberService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<MemberCreateDto> _validator;

    public CreateMemberHandler(IMemberService service, IMapper mapper, IValidator<MemberCreateDto> validator)
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<MemberResponseDto> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request.Dto, cancellationToken);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        var entity = _mapper.Map<Member>(request.Dto);
        var created = await _service.CreateAsync(entity, cancellationToken);
        return _mapper.Map<MemberResponseDto>(created);
    }
}


