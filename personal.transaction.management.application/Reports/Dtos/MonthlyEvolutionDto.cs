namespace personal.transaction.management.application.Reports.Dtos;

public record MonthlyEvolutionDto
{
	public int YearMonth { get; set; }
	public MonthlyEvolutionAccountDto[] Accounts { get; set; } = [];

}

public record MonthlyEvolutionAccountDto
{
	public Guid AccountId { get; init; }
	public string AccountName { get; set; } = string.Empty;
	public decimal Amount { get; init; }
}
