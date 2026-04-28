using Microsoft.EntityFrameworkCore;
using personal.transaction.management.application.Spending;
using personal.transaction.management.application.Spendings.Dtos;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

public class SpendingRepository(ApplicationDbContext context) : ISpendingRepository
{
	public async Task<ICollection<SpedingCategoryAverageDto>> GetSpedingAverageByCategoryAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
	{
		var spendingsAverageQuery = context.Transactions
		.Where(t => t.UserId == userId && t.Date >= from && t.Date <= to)
		.Join(context.Categories, t => t.CategoryId, c => c.Id, (t, c) => new { t, c })
		.GroupBy(tc => new { tc.t.CategoryId, tc.c.Name, tc.t.Amount.Currency })
		.Select(g => new SpedingCategoryAverageDto
		{
			CategoryId = g.Key.CategoryId,
			CategoryName = g.Key.Name,
			Currency = g.Key.Currency.Code,
			Amount = g.Average(x => x.t.Amount.Value),
		});

		return await spendingsAverageQuery.ToListAsync(cancellationToken);
	}

	public async Task<ICollection<SpendingCategoryDto>> GetSpendingsByCategoryAsync(Guid userId, DateOnly from, CancellationToken cancellationToken = default)
	{
		var spendingsQuery = context.Transactions
			.Where(t => t.UserId == userId && t.Date == from)
			.Join(context.Categories, t => t.CategoryId, c => c.Id, (t, c) => new { t, c })
			.GroupBy(tc => new { tc.t.CategoryId, tc.c.Name, tc.t.Amount.Currency })
			.Select(g => new SpendingCategoryDto
			{
				CategoryId = g.Key.CategoryId,
				CategoryName = g.Key.Name,
				Currency = g.Key.Currency.Code,
				Amount = g.Sum(x => x.t.Amount.Value),
				Date = from
			});

		return await spendingsQuery.ToListAsync(cancellationToken);
	}
}
