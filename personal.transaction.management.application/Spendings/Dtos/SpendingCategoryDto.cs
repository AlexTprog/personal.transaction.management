namespace personal.transaction.management.application.Spendings.Dtos;

public record SpendingCategoryDto
{
	public Guid CategoryId { get; init; }
	public string CategoryName { get; init; } = null!;
	public decimal Amount { get; init; }
	public string Currency { get; init; } = null!;
}

public record SpendingAverageCategoryDto
{
	public Guid CategoryId { get; init; }
	public string CategoryName { get; init; } = null!;
	public decimal AverageAmount { get; init; }
	public int MonthsWithData { get; init; }
	public string Currency { get; init; } = null!;
}

public record SpendingAnomalyDto
{
	public Guid CategoryId { get; init; }
	public string CategoryName { get; init; } = null!;
	public decimal Amount { get; init; }
	public string Currency { get; init; } = null!;
	public DateOnly Date { get; init; }
	public int MonthsWithData { get; init; }
	public decimal AverageAmount { get; init; }
	public decimal DeviationFactor { get; init; }
}