using Microsoft.EntityFrameworkCore;
using personal.transaction.management.application.Reports;
using personal.transaction.management.application.Reports.Dtos;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.infrastructure.Persistence.Repositories;

internal sealed class ReportRepository(ApplicationDbContext context) : IReportRepository
{
    public async Task<ICollection<MonthlyEvolutionDto>> GetMonthlyEvolutionAsync(
        Guid userId, DateOnly fromDate, DateOnly toDate,
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
                Income  = g.Sum(x => x.t.TransactionType == TransactionTypeEnum.Income  ? x.t.Amount.Value : 0m),
                Expense = g.Sum(x => x.t.TransactionType == TransactionTypeEnum.Expense ? x.t.Amount.Value : 0m)
            })
            .ToListAsync(cancellationToken);

        return raw
            .GroupBy(x => x.Year * 100 + x.Month)
            .Select(g => new MonthlyEvolutionDto
            {
                YearMonth = g.Key,
                Accounts  = g.Select(x => new MonthlyEvolutionAccountDto
                {
                    AccountId   = x.AccountId,
                    AccountName = x.AccountName,
                    Amount      = x.Income - x.Expense
                }).ToArray()
            })
            .ToList();
    }

    public async Task<PeriodSummaryDto> GetPeriodSummaryAsync(
        Guid userId, DateOnly from, DateOnly to,
        CancellationToken cancellationToken = default)
    {
        var result = await context.Transactions
            .Where(t => t.UserId == userId && t.Date >= from && t.Date <= to
                     && (t.TransactionType == TransactionTypeEnum.Income
                         || t.TransactionType == TransactionTypeEnum.Expense))
            .GroupBy(_ => 0)
            .Select(g => new
            {
                TotalIncome   = g.Sum(t => t.TransactionType == TransactionTypeEnum.Income  ? t.Amount.Value : 0m),
                TotalExpenses = g.Sum(t => t.TransactionType == TransactionTypeEnum.Expense ? t.Amount.Value : 0m)
            })
            .FirstOrDefaultAsync(cancellationToken);

        var income   = result?.TotalIncome   ?? 0m;
        var expenses = result?.TotalExpenses ?? 0m;

        return new PeriodSummaryDto
        {
            From          = from,
            To            = to,
            TotalIncome   = income,
            TotalExpenses = expenses,
            Net           = income - expenses
        };
    }

    public async Task<ExpensesByCategoryDto> GetExpensesByCategoryAsync(
        Guid userId, DateOnly from, DateOnly to,
        CancellationToken cancellationToken = default)
    {
        var items = await context.Transactions
            .Where(t => t.UserId == userId && t.Date >= from && t.Date <= to
                     && t.TransactionType == TransactionTypeEnum.Expense)
            .Join(context.Categories, t => t.CategoryId, c => c.Id, (t, c) => new { t, c })
            .GroupBy(x => new { x.c.Id, x.c.Name, x.c.Icon })
            .Select(g => new
            {
                CategoryId   = g.Key.Id,
                CategoryName = g.Key.Name,
                CategoryIcon = g.Key.Icon,
                Total        = g.Sum(x => x.t.Amount.Value)
            })
            .ToListAsync(cancellationToken);

        var grandTotal = items.Sum(x => x.Total);

        return new ExpensesByCategoryDto
        {
            From = from,
            To   = to,
            Categories = items
                .OrderByDescending(x => x.Total)
                .Select(x => new CategoryBreakdownDto
                {
                    CategoryId   = x.CategoryId,
                    CategoryName = x.CategoryName,
                    CategoryIcon = x.CategoryIcon,
                    Total        = x.Total,
                    Percentage   = grandTotal > 0 ? Math.Round(x.Total / grandTotal * 100, 2) : 0m
                })
                .ToArray()
        };
    }

    public async Task<CurrentBalanceDto> GetCurrentBalanceAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var accounts = await context.Accounts
            .Where(a => a.UserId == userId && a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);

        var accountDtos = accounts.Select(a => new AccountBalanceDto
        {
            AccountId   = a.Id,
            AccountName = a.Name,
            AccountType = a.AccountType.ToString(),
            Currency    = a.Currency.Code,
            Balance     = a.Balance
        }).ToArray();

        return new CurrentBalanceDto { Accounts = accountDtos };
    }
}
