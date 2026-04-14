using Microsoft.EntityFrameworkCore;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class CategoryRepository(ApplicationDbContext context) : Repository<Category>(context), ICategoryRepository
{
	public async Task<IReadOnlyList<Category>> GetByUserIdAsync(
		Guid userId, CancellationToken cancellationToken = default)
		=> await DbSet
			.Where(c => (c.UserId == userId || c.IsSystem) && c.IsActive)
			.OrderBy(c => c.IsSystem)
			.ThenBy(c => c.Name)
			.ToListAsync(cancellationToken);

	public async Task<IReadOnlyList<Category>> GetSystemCategoriesAsync(
		CancellationToken cancellationToken = default)
		=> await DbSet
			.Where(c => c.IsSystem && c.IsActive)
			.OrderBy(c => c.Name)
			.ToListAsync(cancellationToken);

	public async Task<bool> HasTransactionsAsync(
		Guid categoryId, CancellationToken cancellationToken = default)
		=> await Context.Transactions
			.AnyAsync(t => t.CategoryId == categoryId, cancellationToken);
}
