namespace personal.transaction.management.application.Spendings.Dtos;

public record SpendingCategoryDto
{
	public Guid CategoryId { get; init; }
	public string CategoryName { get; init; } = null!;
	public decimal Amount { get; init; }
	public string Currency { get; init; } = null!;
	public DateOnly Date { get; init; }
}

public record SpedingCategoryAverageDto
{
	public Guid CategoryId { get; init; }
	public string CategoryName { get; init; } = null!;
	public decimal Amount { get; init; }
	public string Currency { get; init; } = null!;
}

public record SpendingAnomalyDto
{
	public Guid CategoryId { get; init; }
	public string CategoryName { get; init; } = null!;
	public decimal Amount { get; init; }
	public string Currency { get; init; } = null!;
	public DateOnly Date { get; init; }
	public decimal AverageAmount { get; init; }
}