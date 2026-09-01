
using System.Domain.Entities.BaseClass;

namespace System.Application.Abstractions.UOW
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        IEnumerable<clsBaseClass> GetEntitiesWithDomainEvents();
    }
}
