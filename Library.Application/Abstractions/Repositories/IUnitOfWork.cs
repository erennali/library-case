using System.Threading;
using System.Threading.Tasks;

namespace Library.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


