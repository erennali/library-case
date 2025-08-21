using AutoMapper;
using FluentValidation;
using Library.Application.Abstractions.Services;
using Library.Application.Categories.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Categories.Commands;

public record CreateCategoryCommand(CategoryCreateDto Dto) : IRequest<CategoryResponseDto>;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponseDto>
{
    private readonly ICategoryService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<CategoryCreateDto> _validator;

    public CreateCategoryHandler(ICategoryService service, IMapper mapper, IValidator<CategoryCreateDto> validator)
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CategoryResponseDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request.Dto, cancellationToken);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var entity = _mapper.Map<Category>(request.Dto);
        var created = await _service.CreateAsync(entity, cancellationToken);
        return _mapper.Map<CategoryResponseDto>(created);
    }
}


