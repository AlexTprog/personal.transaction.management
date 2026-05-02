using Microsoft.EntityFrameworkCore;
using personal.transaction.management.application.Spendings;
using personal.transaction.management.application.Spendings.Dtos;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class SpendingRepository(ApplicationDbContext context) : ISpendingRepository
{
	public async Task<ICollection<SpendingCategoryAverageDto>> GetSpendingAverageByCategoriesAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
	{
		var spendingsAverageQuery = context.Transactions
		.Where(t => t.UserId == userId && t.Date >= from && t.Date <= to && t.TransactionType == TransactionTypeEnum.Expense)
		.Join(context.Categories, t => t.CategoryId, c => c.Id, (t, c) => new { t, c })
		.GroupBy(tc => new { tc.t.CategoryId, tc.c.Name, tc.t.Amount.Currency, tc.t.Date.Year, tc.t.Date.Month })
		.Select(g => new SpendingCategoryAverageDto
		{
			CategoryId = g.Key.CategoryId,
			CategoryName = g.Key.Name,
			Currency = g.Key.Currency.Code,
			Amount = g.Sum(x => x.t.Amount.Value),
		});

		var spendingsAverageList = await spendingsAverageQuery.ToListAsync(cancellationToken);

		var result = spendingsAverageList.GroupBy(s => new { s.CategoryId, s.CategoryName, s.Currency })
		.Select(g => new SpendingCategoryAverageDto
		{
			CategoryId = g.Key.CategoryId,
			CategoryName = g.Key.CategoryName,
			Currency = g.Key.Currency,
			Amount = g.Average(x => x.Amount),
		}).ToList();

		return result;
	}

	public async Task<ICollection<SpendingCategoryDto>> GetSpendingByCategoriesAsync(Guid userId, DateOnly from, CancellationToken cancellationToken = default)
	{
		var spendingsQuery = context.Transactions
			.Where(t => t.UserId == userId && t.Date.Year == from.Year && t.Date.Month == from.Month && t.TransactionType == TransactionTypeEnum.Expense)
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
