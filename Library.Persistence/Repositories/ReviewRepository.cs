using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Include(r => r.Book)
            .Include(r => r.Member)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Review> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Reviews
            .Include(r => r.Book)
            .Include(r => r.Member)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r =>
                r.Book.Title.Contains(search) ||
                r.Member.FirstName.Contains(search) ||
                r.Member.LastName.Contains(search) ||
                (r.Comment != null && r.Comment.Contains(search)));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.ReviewDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Review> Items, int TotalCount)> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Reviews
            .Include(r => r.Member)
            .Where(r => r.BookId == bookId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.ReviewDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Review> Items, int TotalCount)> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Reviews
            .Include(r => r.Book)
            .Where(r => r.MemberId == memberId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.ReviewDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Review> CreateAsync(Review review, CancellationToken cancellationToken = default)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);
        return review;
    }

    public async Task<Review> UpdateAsync(Review review, CancellationToken cancellationToken = default)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync(cancellationToken);
        return review;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var review = await _context.Reviews.FindAsync(new object[] { id }, cancellationToken);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<double> GetAverageRatingAsync(int bookId, CancellationToken cancellationToken = default)
    {
        var reviews = await _context.Reviews
            .Where(r => r.BookId == bookId && r.IsApproved)
            .ToListAsync(cancellationToken);

        return reviews.Any() ? reviews.Average(r => r.Rating) : 0.0;
    }

    public async Task<int> GetReviewCountAsync(int bookId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .CountAsync(r => r.BookId == bookId && r.IsApproved, cancellationToken);
    }
}



