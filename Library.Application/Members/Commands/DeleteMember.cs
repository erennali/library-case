using Library.Application.Abstractions.Services;
using MediatR;

namespace Library.Application.Members.Commands;

public record DeleteMemberCommand(int Id) : IRequest;

public class DeleteMemberHandler : IRequestHandler<DeleteMemberCommand>
{
    private readonly IMemberService _service;

    public DeleteMemberHandler(IMemberService service)
    {
        _service = service;
    }

    public async Task Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request.Id, cancellationToken);
    }
}


