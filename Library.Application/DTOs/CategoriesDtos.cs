namespace Library.Application.DTOs;

public record ReorderCategoriesRequest(
    List<CategoryOrderItem> Categories
);

public record MoveCategoryRequest(
    int? NewParentId
);

public record CategoryOrderItem(
    int Id,
    int Order
);

public record CategoryResponse(
    int Id,
    string Name,
    string? Description,
    int? ParentCategoryId,
    bool IsActive,
    DateTime CreatedAt,
    int? SubCategoryCount,
    int BookCount
);

public record CreateCategoryRequest(
    string Name,
    string? Description,
    int? ParentCategoryId
);

public record UpdateCategoryRequest(
    string Name,
    string? Description,
    int? ParentCategoryId,
    bool IsActive
);
