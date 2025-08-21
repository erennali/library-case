using Library.Application.Abstractions.Repositories;
using Library.Application.Abstractions.Services;
using Library.Domain.Entities;

namespace Library.Application.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repo;
    private readonly IUnitOfWork _uow;

    public MemberService(IMemberRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public Task<Member?> GetAsync(int id, CancellationToken cancellationToken = default)
        => _repo.GetByIdAsync(id, cancellationToken);

    public async Task<(IReadOnlyList<Member> Items, int TotalCount)> SearchAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        var itemsTask = _repo.GetPagedAsync(page, pageSize, search, cancellationToken);
        var countTask = _repo.CountAsync(search, cancellationToken);
        await Task.WhenAll(itemsTask, countTask);
        return (await itemsTask, await countTask);
    }

    public async Task<Member> CreateAsync(Member member, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(member.MembershipNumber)) throw new ArgumentException("MembershipNumber is required");
        if (string.IsNullOrWhiteSpace(member.Email)) throw new ArgumentException("Email is required");
        member.CreatedAt = DateTime.UtcNow;
        member.UpdatedAt = DateTime.UtcNow;
        await _repo.AddAsync(member, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return member;
    }

    public async Task<Member> UpdateAsync(int id, Member updated, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByIdAsync(id, cancellationToken);
        if (existing is null) throw new KeyNotFoundException($"Member {id} not found");
        existing.MembershipNumber = updated.MembershipNumber;
        existing.FirstName = updated.FirstName;
        existing.LastName = updated.LastName;
        existing.Email = updated.Email;
        existing.PhoneNumber = updated.PhoneNumber;
        existing.Address = updated.Address;
        existing.DateOfBirth = updated.DateOfBirth;
        existing.MembershipType = updated.MembershipType;
        existing.MembershipStartDate = updated.MembershipStartDate;
        existing.MembershipEndDate = updated.MembershipEndDate;
        existing.Status = updated.Status;
        existing.MaxBooksAllowed = updated.MaxBooksAllowed;
        existing.CurrentBooksCount = updated.CurrentBooksCount;
        existing.TotalFinesOwed = updated.TotalFinesOwed;
        existing.MaxFineLimit = updated.MaxFineLimit;
        existing.UpdatedAt = DateTime.UtcNow;
        _repo.Update(existing);
        await _uow.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByIdAsync(id, cancellationToken);
        if (existing is null) return;
        _repo.Remove(existing);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}


