using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Review> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Review> Items, int TotalCount)> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Review> Items, int TotalCount)> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Review> CreateAsync(Review review, CancellationToken cancellationToken = default);
    Task<Review> UpdateAsync(Review review, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<double> GetAverageRatingAsync(int bookId, CancellationToken cancellationToken = default);
    Task<int> GetReviewCountAsync(int bookId, CancellationToken cancellationToken = default);
}

