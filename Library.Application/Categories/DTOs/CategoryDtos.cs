namespace Library.Application.Categories.DTOs;

public record CategoryCreateDto(
    string Name,
    string? Description,
    int? ParentCategoryId
);

public record CategoryUpdateDto(
    string Name,
    string? Description,
    int? ParentCategoryId
);

public record CategoryResponseDto(
    int Id,
    string Name,
    string? Description,
    int? ParentCategoryId,
    bool IsActive,
    DateTime CreatedAt
);


