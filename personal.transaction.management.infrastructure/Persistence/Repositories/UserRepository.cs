using Microsoft.EntityFrameworkCore;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class UserRepository(ApplicationDbContext context) : Repository<User>(context), IUserRepository
{
	public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		var normalized = Email.From(email.Trim().ToLowerInvariant());
		return await DbSet.FirstOrDefaultAsync(u => u.Email.Equals(normalized), cancellationToken);
	}

	public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		var normalized = Email.From(email.Trim().ToLowerInvariant());
		return await DbSet.AnyAsync(u => u.Email.Equals(normalized), cancellationToken);
	}
}
