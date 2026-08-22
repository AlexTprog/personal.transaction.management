using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class BudgetRepository(ApplicationDbContext context) : Repository<Budget>(context), IBudgetRepository
{
}
