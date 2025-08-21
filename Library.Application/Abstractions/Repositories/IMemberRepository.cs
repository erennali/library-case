using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string? search, CancellationToken cancellationToken = default);
    Task AddAsync(Member member, CancellationToken cancellationToken = default);
    void Update(Member member);
    void Remove(Member member);
}


