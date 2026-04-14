using personal.transaction.management.domain.entities;

namespace personal.transaction.management.domain.repositories;

public interface ICategoryRepository : IRepository<Category>
{
	Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<Category>> GetSystemCategoriesAsync(CancellationToken cancellationToken = default);
	Task<bool> HasTransactionsAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
