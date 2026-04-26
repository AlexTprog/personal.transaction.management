using personal.transaction.management.domain.entities;

namespace personal.transaction.management.domain.repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
	Task<Transaction?> GetByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<Transaction>> GetByTransferIdAsync(Guid transferId, CancellationToken cancellationToken = default);
	Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedByUserAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
}
