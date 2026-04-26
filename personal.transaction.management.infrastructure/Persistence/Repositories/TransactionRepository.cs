using Microsoft.EntityFrameworkCore;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class TransactionRepository(ApplicationDbContext context) : Repository<Transaction>(context), ITransactionRepository
{
	public async Task<Transaction?> GetByIdAndUserIdAsync(
		Guid id, Guid userId, CancellationToken cancellationToken = default)
		=> await DbSet
			.Include(t => t.Tags)
			.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellationToken);

	public async Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(
		Guid accountId, CancellationToken cancellationToken = default)
		=> await DbSet
			.Where(t => t.AccountId == accountId)
			.Include(t => t.Tags)
			.OrderByDescending(t => t.Date)
			.ToListAsync(cancellationToken);

	public async Task<IReadOnlyList<Transaction>> GetByTransferIdAsync(
		Guid transferId, CancellationToken cancellationToken = default)
		=> await DbSet
			.Where(t => t.TransferId == transferId)
			.ToListAsync(cancellationToken);

	public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedByUserAsync(
		Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
	{
		var query = DbSet
			.Where(t => t.UserId == userId)
			.OrderByDescending(t => t.Date)
			.ThenByDescending(t => t.CreatedAt);

		var totalCount = await query.CountAsync(cancellationToken);

		var items = await query
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Include(t => t.Tags)
			.ToListAsync(cancellationToken);

		return (items, totalCount);
	}

}
