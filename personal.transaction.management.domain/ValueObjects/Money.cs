using personal.transaction.management.domain.exceptions;

namespace personal.transaction.management.domain.valueobjects;

public sealed record Money
{
	public decimal Value { get; }
	public Currency Currency { get; }

	private Money(decimal value, Currency currency)
	{
		Value = value;
		Currency = currency;
	}

	public static Money Of(decimal value, string currencyCode)
	{
		if (value <= 0)
			throw new DomainValidationException("Amount", "Amount must be greater than zero.");

		return new Money(value, Currency.From(currencyCode));
	}

	public static Money Of(decimal value, Currency currency)
	{
		if (value <= 0)
			throw new DomainValidationException("Amount", "Amount must be greater than zero.");

		return new Money(value, currency);
	}

	public override string ToString() => $"{Value} {Currency.Code}";
}
