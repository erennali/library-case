using Library.Domain.Enums;

namespace Library.Application.Books.DTOs;

public record BookCreateDto(
    string ISBN,
    string Title,
    string Author,
    string? Publisher,
    DateTime? PublicationDate,
    string? Description,
    int CategoryId,
    int TotalCopies,
    int AvailableCopies,
    string? Language,
    int PageCount,
    string? ImageUrl,
    decimal? Price,
    BookStatus Status
);

public record BookUpdateDto(
    string ISBN,
    string Title,
    string Author,
    string? Publisher,
    DateTime? PublicationDate,
    string? Description,
    int CategoryId,
    int TotalCopies,
    int AvailableCopies,
    string? Language,
    int PageCount,
    string? ImageUrl,
    decimal? Price,
    BookStatus Status
);

public record BookResponseDto(
    int Id,
    string ISBN,
    string Title,
    string Author,
    string? Publisher,
    DateTime? PublicationDate,
    string? Description,
    int CategoryId,
    int TotalCopies,
    int AvailableCopies,
    string? Language,
    int PageCount,
    string? ImageUrl,
    decimal? Price,
    BookStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);


