using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.application.Accounts.Dtos;

public record AccountDto
{
	public Guid Id { get; init; }
	public Guid UserId { get; init; }
	public string Name { get; init; } = string.Empty;
	public AccountTypeEnum AccountType { get; init; }
	public string Currency { get; init; } = string.Empty;
	public decimal Balance { get; init; }
	public bool IsActive { get; init; }
	public DateTime CreatedAt { get; init; }

	public static AccountDto FromEntity(Account account) => new()
	{
		Id = account.Id,
		UserId = account.UserId,
		Name = account.Name,
		AccountType = account.AccountType,
		Currency = account.Currency.Code,
		Balance = account.Balance,
		IsActive = account.IsActive,
		CreatedAt = account.CreatedAt
	};
};

