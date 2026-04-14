namespace personal.transaction.management.domain.repositories;

public interface IUnitOfWork
{
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
