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

    public Task AddAsync(Member member, CancellationToken cancellationToken = default)
        => _dbContext.Members.AddAsync(member, cancellationToken).AsTask();

    public void Update(Member member) => _dbContext.Members.Update(member);

    public void Remove(Member member) => _dbContext.Members.Remove(member);
}


