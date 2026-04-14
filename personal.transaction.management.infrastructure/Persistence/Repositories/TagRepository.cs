using Microsoft.EntityFrameworkCore;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class TagRepository(ApplicationDbContext context) : Repository<Tag>(context), ITagRepository
{
	public async Task<IReadOnlyList<Tag>> GetByUserIdAsync(
		Guid userId, CancellationToken cancellationToken = default)
		=> await DbSet
			.Where(t => t.UserId == userId || t.IsSystem)
			.OrderBy(t => t.IsSystem)
			.ThenBy(t => t.Name)
			.ToListAsync(cancellationToken);

	public async Task<IReadOnlyList<Tag>> GetSystemTagsAsync(
		CancellationToken cancellationToken = default)
		=> await DbSet
			.Where(t => t.IsSystem)
			.OrderBy(t => t.Name)
			.ToListAsync(cancellationToken);

	public async Task<bool> IsInUseAsync(
		Guid tagId, CancellationToken cancellationToken = default)
		=> await Context.TransactionTags
			.AnyAsync(tt => tt.TagId == tagId, cancellationToken);
}
