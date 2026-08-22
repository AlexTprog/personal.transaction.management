namespace personal.transaction.management.application.Reports.Dtos;

public record PeriodSummaryDto
{
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }
    public decimal TotalIncome { get; init; }
    public decimal TotalExpenses { get; init; }
    public decimal Net { get; init; }
}
