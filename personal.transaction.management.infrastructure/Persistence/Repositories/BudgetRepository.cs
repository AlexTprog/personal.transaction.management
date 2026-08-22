using personal.transaction.management.domain.Entities;
using personal.transaction.management.domain.Repositories;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class BudgetRepository(ApplicationDbContext context) : Repository<Budget>(context), IBudgetRepository
{
}
