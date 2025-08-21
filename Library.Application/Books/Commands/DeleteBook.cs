using Library.Application.Abstractions.Services;
using MediatR;

namespace Library.Application.Books.Commands;

public record DeleteBookCommand(int Id) : IRequest;

public class DeleteBookHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly IBookService _service;

    public DeleteBookHandler(IBookService service)
    {
        _service = service;
    }

    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request.Id, cancellationToken);
    }
}


