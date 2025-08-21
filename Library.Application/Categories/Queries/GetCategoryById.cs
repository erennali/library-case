using AutoMapper;
using Library.Application.Abstractions.Services;
using Library.Application.Categories.DTOs;
using MediatR;

namespace Library.Application.Categories.Queries;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryResponseDto?>;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponseDto?>
{
    private readonly ICategoryService _service;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<CategoryResponseDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var cat = await _service.GetAsync(request.Id, cancellationToken);
        return cat is null ? null : _mapper.Map<CategoryResponseDto>(cat);
    }
}


