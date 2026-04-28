namespace personal.transaction.management.application.Reports.Dtos;

public record ExpensesByCategoryDto
{
	public DateOnly From { get; init; }
	public DateOnly To { get; init; }
	public CategoryBreakdownDto[] Categories { get; init; } = [];
}

public record CategoryBreakdownDto
{
	public Guid CategoryId { get; init; }
	public string CategoryName { get; init; } = string.Empty;
	public string CategoryIcon { get; init; } = string.Empty;
	public decimal Total { get; init; }
	public decimal Percentage { get; init; }
}
