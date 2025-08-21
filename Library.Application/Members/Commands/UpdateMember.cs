using AutoMapper;
using FluentValidation;
using Library.Application.Abstractions.Services;
using Library.Application.Members.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Members.Commands;

public record UpdateMemberCommand(int Id, MemberUpdateDto Dto) : IRequest<MemberResponseDto>;

public class UpdateMemberHandler : IRequestHandler<UpdateMemberCommand, MemberResponseDto>
{
    private readonly IMemberService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<MemberUpdateDto> _validator;

    public UpdateMemberHandler(IMemberService service, IMapper mapper, IValidator<MemberUpdateDto> validator)
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<MemberResponseDto> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request.Dto, cancellationToken);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        var entity = _mapper.Map<Member>(request.Dto);
        var updated = await _service.UpdateAsync(request.Id, entity, cancellationToken);
        return _mapper.Map<MemberResponseDto>(updated);
    }
}


