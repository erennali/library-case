using Library.Application.Abstractions.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext _dbContext;

    public MemberRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Member>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        IQueryable<Member> query = _dbContext.Members.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(m => m.FirstName.Contains(s) || m.LastName.Contains(s) || m.Email.Contains(s) || m.MembershipNumber.Contains(s));
        }
        return await query.OrderBy(m => m.LastName).ThenBy(m => m.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(string? search, CancellationToken cancellationToken = default)
    {
        IQueryable<Member> query = _dbContext.Members.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(m => m.FirstName.Contains(s) || m.LastName.Contains(s) || m.Email.Contains(s) || m.MembershipNumber.Contains(s));
        }
        return await query.CountAsync(cancellationToken);
    }

    public async Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Email == email, cancellationToken);
    }

    public async Task<Member?> GetByMembershipNumberAsync(string membershipNumber, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MembershipNumber == membershipNumber, cancellationToken);
    }

    public async Task<Member> CreateAsync(Member member, CancellationToken cancellationToken = default)
    {
        _dbContext.Members.Add(member);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return member;
    }

    public async Task<Member> UpdateAsync(Member member, CancellationToken cancellationToken = default)
    {
        _dbContext.Members.Update(member);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return member;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await GetByIdAsync(id, cancellationToken);
        if (member != null)
        {
            _dbContext.Members.Remove(member);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Members.AnyAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Members.AnyAsync(m => m.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByMembershipNumberAsync(string membershipNumber, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Members.AnyAsync(m => m.MembershipNumber == membershipNumber, cancellationToken);
    }
}


