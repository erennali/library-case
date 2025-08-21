using Library.Application.Abstractions.Services;
using MediatR;

namespace Library.Application.Categories.Commands;

public record DeleteCategoryCommand(int Id) : IRequest;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryService _service;

    public DeleteCategoryHandler(ICategoryService service)
    {
        _service = service;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request.Id, cancellationToken);
    }
}


