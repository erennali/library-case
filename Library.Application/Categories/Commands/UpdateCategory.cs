using AutoMapper;
using FluentValidation;
using Library.Application.Abstractions.Services;
using Library.Application.Categories.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Categories.Commands;

public record UpdateCategoryCommand(int Id, CategoryUpdateDto Dto) : IRequest<CategoryResponseDto>;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponseDto>
{
    private readonly ICategoryService _service;
    private readonly IMapper _mapper;
    private readonly IValidator<CategoryUpdateDto> _validator;

    public UpdateCategoryHandler(ICategoryService service, IMapper mapper, IValidator<CategoryUpdateDto> validator)
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CategoryResponseDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request.Dto, cancellationToken);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var entity = _mapper.Map<Category>(request.Dto);
        var updated = await _service.UpdateAsync(request.Id, entity, cancellationToken);
        return _mapper.Map<CategoryResponseDto>(updated);
    }
}


