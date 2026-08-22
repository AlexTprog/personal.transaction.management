namespace personal.transaction.management.application.Reports.Dtos;

public record CurrentBalanceDto
{
    public AccountBalanceDto[] Accounts { get; init; } = [];
}

public record AccountBalanceDto
{
    public Guid AccountId { get; init; }
    public string AccountName { get; init; } = string.Empty;
    public string AccountType { get; init; } = string.Empty;
    public string Currency { get; init; } = string.Empty;
    public decimal Balance { get; init; }
}
