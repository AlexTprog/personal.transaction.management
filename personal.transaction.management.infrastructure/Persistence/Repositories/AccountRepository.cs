using Microsoft.EntityFrameworkCore;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class AccountRepository(ApplicationDbContext context) : Repository<Account>(context), IAccountRepository
{
	public async Task<IReadOnlyList<Account>> GetByUserIdAsync(
		Guid userId, CancellationToken cancellationToken = default)
		=> await DbSet
			.Where(a => a.UserId == userId && a.IsActive)
			.OrderBy(a => a.Name)
			.ToListAsync(cancellationToken);

	public async Task<Account?> GetByIdAndUserIdAsync(
		Guid id, Guid userId, CancellationToken cancellationToken = default)
		=> await DbSet
			.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, cancellationToken);
}
