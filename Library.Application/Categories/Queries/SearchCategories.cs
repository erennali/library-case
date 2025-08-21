using AutoMapper;
using Library.Application.Abstractions.Services;
using Library.Application.Categories.DTOs;
using Library.Application.Common.Pagination;
using MediatR;

namespace Library.Application.Categories.Queries;

public record SearchCategoriesQuery(int Page, int PageSize, string? Query, int? ParentCategoryId) : IRequest<PagedResult<CategoryResponseDto>>;

public class SearchCategoriesHandler : IRequestHandler<SearchCategoriesQuery, PagedResult<CategoryResponseDto>>
{
    private readonly ICategoryService _service;
    private readonly IMapper _mapper;

    public SearchCategoriesHandler(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<PagedResult<CategoryResponseDto>> Handle(SearchCategoriesQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _service.SearchAsync(request.Page, request.PageSize, request.Query, request.ParentCategoryId, cancellationToken);
        var mapped = items.Select(_mapper.Map<CategoryResponseDto>).ToList();
        return new PagedResult<CategoryResponseDto>(mapped, total, request.Page, request.PageSize);
    }
}


