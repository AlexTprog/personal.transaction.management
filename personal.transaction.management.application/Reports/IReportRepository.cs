using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports;

public interface IReportRepository
{
	Task<ICollection<MonthlyEvolutionDto>> GetMonthlyEvolutionAsync(
		Guid userId, DateOnly fromDate, DateOnly toDate,
		CancellationToken cancellationToken = default);

	Task<PeriodSummaryDto> GetPeriodSummaryAsync(
		Guid userId, DateOnly from, DateOnly to,
		CancellationToken cancellationToken = default);

	Task<ExpensesByCategoryDto> GetExpensesByCategoryAsync(
		Guid userId, DateOnly from, DateOnly to,
		CancellationToken cancellationToken = default);

	Task<CurrentBalanceDto> GetCurrentBalanceAsync(
		Guid userId,
		CancellationToken cancellationToken = default);
}
