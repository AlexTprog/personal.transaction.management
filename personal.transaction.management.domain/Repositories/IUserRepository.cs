using personal.transaction.management.domain.entities;

namespace personal.transaction.management.domain.repositories;

public interface IUserRepository : IRepository<User>
{
	Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
	Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}
