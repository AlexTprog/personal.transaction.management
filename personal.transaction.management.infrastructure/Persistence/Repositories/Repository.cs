using Microsoft.EntityFrameworkCore;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal abstract class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class
{
	protected readonly ApplicationDbContext Context = context;
	protected readonly DbSet<T> DbSet = context.Set<T>();

	public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		=> await DbSet.FindAsync([id], cancellationToken);

	public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
		=> await DbSet.AddRangeAsync(entities, cancellationToken);

	public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
		=> await DbSet.AddAsync(entity, cancellationToken);

	public void Update(T entity)
		=> DbSet.Update(entity);

	public void Remove(T entity)
		=> DbSet.Remove(entity);
}
