using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.Member)
            .Include(r => r.ProcessedByLibrarian)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Reservation> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.Member)
            .Include(r => r.ProcessedByLibrarian)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r =>
                r.Book.Title.Contains(search) ||
                r.Member.FirstName.Contains(search) ||
                r.Member.LastName.Contains(search) ||
                r.ReservationNumber.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.ReservationDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Reservation> Items, int TotalCount)> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.ProcessedByLibrarian)
            .Where(r => r.MemberId == memberId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.ReservationDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Reservation> Items, int TotalCount)> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Reservations
            .Include(r => r.Member)
            .Include(r => r.ProcessedByLibrarian)
            .Where(r => r.BookId == bookId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.ReservationDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Reservation> Items, int TotalCount)> GetByStatusAsync(ReservationStatus status, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.Member)
            .Include(r => r.ProcessedByLibrarian)
            .Where(r => r.Status == status);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.ReservationDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);
        return reservation;
    }

    public async Task<Reservation> UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync(cancellationToken);
        return reservation;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var reservation = await _context.Reservations.FindAsync(new object[] { id }, cancellationToken);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<int> CountActiveReservationsAsync(int bookId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .CountAsync(r => r.BookId == bookId && r.Status == ReservationStatus.Active, cancellationToken);
    }

    public async Task<bool> HasActiveReservationAsync(int memberId, int bookId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .AnyAsync(r => r.MemberId == memberId && r.BookId == bookId && r.Status == ReservationStatus.Active, cancellationToken);
    }
}



