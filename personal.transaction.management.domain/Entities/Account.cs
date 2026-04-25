using personal.transaction.management.domain.abstractions;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.events;
using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.entities;

public class Account : BaseAuditable
{
	public Guid Id { get; private set; }
	public Guid UserId { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public AccountTypeEnum AccountType { get; private set; }
	public Currency Currency { get; private set; } = null!;
	public decimal Balance { get; private set; } = 0m;
	public bool IsActive { get; private set; }

	// Required by EF Core
	private Account() { }

	private Account(Guid userId, string name, AccountTypeEnum accountType, Money money)
	{
		Id = Guid.NewGuid();
		UserId = userId;
		Name = name;
		AccountType = accountType;
		Currency = money.Currency;
		Balance = money.Amount;
		IsActive = true;
	}

	public static Account Create(
		Guid userId, string name, AccountTypeEnum accountType,
		Money money)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new DomainValidationException("Name", "Account name cannot be empty.");

		return new Account(userId, name.Trim(), accountType, money);
	}

	public void Credit(Money amount)
	{
		var previousBalance = Balance;
		Balance += amount.Amount;

		RaiseDomainEvent(new AccountBalanceUpdatedEvent(Id, UserId, previousBalance, Balance));
	}

	public void Debit(Money amount)
	{
		if (AccountType == AccountTypeEnum.Cash && Balance < amount.Amount)
			throw new InsufficientFundsException(Balance, amount.Amount, Currency.Code);

		var previousBalance = Balance;
		Balance -= amount.Amount;

		RaiseDomainEvent(new AccountBalanceUpdatedEvent(Id, UserId, previousBalance, Balance));
	}

	public void Rename(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new DomainValidationException("Name", "Account name cannot be empty.");

		Name = name.Trim();
	}

	public void Deactivate()
	{
		IsActive = false;
	}
}
