using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Member?> GetByMembershipNumberAsync(string membershipNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string? search, CancellationToken cancellationToken = default);
    Task<Member> CreateAsync(Member member, CancellationToken cancellationToken = default);
    Task<Member> UpdateAsync(Member member, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByMembershipNumberAsync(string membershipNumber, CancellationToken cancellationToken = default);
}


