using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class LibrarianRepository : ILibrarianRepository
{
    private readonly AppDbContext _context;

    public LibrarianRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Librarian?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Librarians
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Librarian?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Librarians
            .FirstOrDefaultAsync(l => l.Email == email, cancellationToken);
    }

    public async Task<Librarian?> GetByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Librarians
            .FirstOrDefaultAsync(l => l.EmployeeNumber == employeeNumber, cancellationToken);
    }

    public async Task<(IReadOnlyList<Librarian> Items, int TotalCount)> SearchAsync(int page, int pageSize, string? search, string? role, string? status, CancellationToken cancellationToken = default)
    {
        var query = _context.Librarians.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(l =>
                l.FirstName.Contains(search) ||
                l.LastName.Contains(search) ||
                l.Email.Contains(search) ||
                l.EmployeeNumber.Contains(search));
        }

        // Apply role filter
        if (!string.IsNullOrWhiteSpace(role) && Enum.TryParse<LibrarianRole>(role, out var roleEnum))
        {
            query = query.Where(l => l.Role == roleEnum);
        }

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<LibrarianStatus>(status, out var statusEnum))
        {
            query = query.Where(l => l.Status == statusEnum);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(l => l.LastName)
            .ThenBy(l => l.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Librarian> CreateAsync(Librarian librarian, CancellationToken cancellationToken = default)
    {
        _context.Librarians.Add(librarian);
        await _context.SaveChangesAsync(cancellationToken);
        return librarian;
    }

    public async Task<Librarian> UpdateAsync(Librarian librarian, CancellationToken cancellationToken = default)
    {
        _context.Librarians.Update(librarian);
        await _context.SaveChangesAsync(cancellationToken);
        return librarian;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var librarian = await GetByIdAsync(id, cancellationToken);
        if (librarian != null)
        {
            _context.Librarians.Remove(librarian);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Librarians.AnyAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Librarians.AnyAsync(l => l.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Librarians.AnyAsync(l => l.EmployeeNumber == employeeNumber, cancellationToken);
    }
}

