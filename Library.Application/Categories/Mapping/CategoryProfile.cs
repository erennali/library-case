using AutoMapper;
using Library.Application.Categories.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Categories.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
        CreateMap<Category, CategoryResponseDto>();
    }
}


