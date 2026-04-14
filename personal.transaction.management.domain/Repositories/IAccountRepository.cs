using personal.transaction.management.domain.entities;

namespace personal.transaction.management.domain.repositories;

public interface IAccountRepository : IRepository<Account>
{
	Task<IReadOnlyList<Account>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
	Task<Account?> GetByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
}
