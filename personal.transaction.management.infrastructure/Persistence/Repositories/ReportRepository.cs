using Microsoft.EntityFrameworkCore;
using personal.transaction.management.application.Reports;
using personal.transaction.management.application.Reports.Dtos;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class ReportRepository(ApplicationDbContext context) : IReportRepository
{
    public async Task<ICollection<MonthlyEvolutionDto>> GetMonthlyEvolutionAsync(
        Guid userId,
        DateOnly fromDate,
        DateOnly toDate,
        CancellationToken cancellationToken = default)
    {
        var raw = await context.Transactions
            .Where(t => t.UserId == userId && t.Date >= fromDate && t.Date <= toDate)
            .Join(context.Accounts, t => t.AccountId, a => a.Id, (t, a) => new { t, a })
            .GroupBy(x => new { x.t.Date.Year, x.t.Date.Month, x.t.AccountId, AccountName = x.a.Name })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                g.Key.AccountId,
                g.Key.AccountName,
                Income = g.Sum(x => x.t.TransactionType == TransactionTypeEnum.Income ? x.t.Amount.Value : 0m),
                Expense = g.Sum(x => x.t.TransactionType == TransactionTypeEnum.Expense ? x.t.Amount.Value : 0m)
            })
            .ToListAsync(cancellationToken);

        return raw
            .GroupBy(x => x.Year * 100 + x.Month)
            .Select(g => new MonthlyEvolutionDto
            {
                YearMonth = g.Key,
                Accounts = g.Select(x => new MonthlyEvolutionAccountDto
                {
                    AccountId = x.AccountId,
                    AccountName = x.AccountName,
                    Amount = x.Income - x.Expense
                }).ToArray()
            })
            .ToList();
    }
}
