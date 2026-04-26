using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports;

public interface IReportRepository
{
    Task<ICollection<MonthlyEvolutionDto>> GetMonthlyEvolutionAsync(
        Guid userId,
        DateOnly fromDate,
        DateOnly toDate,
        CancellationToken cancellationToken = default);
}
