using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class SettingsRepository : ISettingsRepository
{
    private readonly AppDbContext _context;

    public SettingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LibrarySettings?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.LibrarySettings
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<LibrarySettings?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.LibrarySettings
            .FirstOrDefaultAsync(s => s.Key == name, cancellationToken);
    }

    public async Task<IReadOnlyList<LibrarySettings>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LibrarySettings
            .OrderBy(s => s.Key)
            .ToListAsync(cancellationToken);
    }

    public async Task<LibrarySettings> CreateAsync(LibrarySettings settings, CancellationToken cancellationToken = default)
    {
        _context.LibrarySettings.Add(settings);
        await _context.SaveChangesAsync(cancellationToken);
        return settings;
    }

    public async Task<LibrarySettings> UpdateAsync(LibrarySettings settings, CancellationToken cancellationToken = default)
    {
        _context.LibrarySettings.Update(settings);
        await _context.SaveChangesAsync(cancellationToken);
        return settings;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var settings = await _context.LibrarySettings.FindAsync(new object[] { id }, cancellationToken);
        if (settings != null)
        {
            _context.LibrarySettings.Remove(settings);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.LibrarySettings.AnyAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.LibrarySettings.AnyAsync(s => s.Key == name, cancellationToken);
    }
}



