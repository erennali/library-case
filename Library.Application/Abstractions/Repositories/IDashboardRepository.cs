using Library.Domain.Entities;

namespace Library.Application.Abstractions.Repositories;

public interface IDashboardRepository
{
    Task<int> GetTotalBooksAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalMembersAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalTransactionsAsync(CancellationToken cancellationToken = default);
    Task<int> GetOverdueTransactionsCountAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetTotalFinesAsync(CancellationToken cancellationToken = default);
    Task<int> GetActiveReservationsCountAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Book>> GetPopularBooksAsync(int count, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> GetTopMembersAsync(int count, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetBooksByCategoryAsync(CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetTransactionsByMonthAsync(int year, CancellationToken cancellationToken = default);
}

