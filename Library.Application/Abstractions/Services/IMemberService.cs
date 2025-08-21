using Library.Domain.Entities;

namespace Library.Application.Abstractions.Services;

public interface IMemberService
{
    Task<Member?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Member> Items, int TotalCount)> SearchAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<Member> CreateAsync(Member member, CancellationToken cancellationToken = default);
    Task<Member> UpdateAsync(int id, Member updated, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}


