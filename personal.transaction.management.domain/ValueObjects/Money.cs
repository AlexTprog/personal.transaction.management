using personal.transaction.management.domain.exceptions;

namespace personal.transaction.management.domain.valueobjects;

public sealed record Money
{
	public decimal Amount { get; }
	public Currency Currency { get; }

	private Money(decimal amount, Currency currency)
	{
		Amount = amount;
		Currency = currency;
	}

	public static Money Of(decimal amount, string currencyCode)
	{
		if (amount <= 0)
			throw new DomainValidationException("Amount", "Amount must be greater than zero.");

		return new Money(amount, Currency.From(currencyCode));
	}

	public static Money Of(decimal amount, Currency currency)
	{
		if (amount <= 0)
			throw new DomainValidationException("Amount", "Amount must be greater than zero.");

		return new Money(amount, currency);
	}

	public override string ToString() => $"{Amount} {Currency.Code}";
}
