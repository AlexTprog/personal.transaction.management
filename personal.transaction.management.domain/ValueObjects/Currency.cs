using personal.transaction.management.domain.exceptions;

namespace personal.transaction.management.domain.valueobjects;

public sealed record Currency
{
	// Subset of ISO 4217 — sufficient for portfolio scope
	private static readonly HashSet<string> ValidCodes = new(StringComparer.OrdinalIgnoreCase)
	{
		"AED", "ARS", "AUD", "BDT", "BGN", "BRL", "CAD", "CHF", "CLP", "CNY",
		"COP", "CZK", "DKK", "EGP", "EUR", "GBP", "HKD", "HUF", "IDR", "ILS",
		"INR", "ISK", "JPY", "KRW", "MXN", "MYR", "NGN", "NOK", "NZD", "PEN",
		"PHP", "PKR", "PLN", "QAR", "RON", "RUB", "SAR", "SEK", "SGD", "THB",
		"TRY", "TWD", "UAH", "USD", "UYU", "VND", "ZAR"
	};

	public static IEnumerable<string> Availables = ValidCodes.AsEnumerable();

	public string Code { get; }

	private Currency(string code) => Code = code;

	public static Currency From(string code)
	{
		if (string.IsNullOrWhiteSpace(code))
			throw new DomainValidationException("Currency", "Currency code cannot be empty.");

		var normalized = code.Trim().ToUpperInvariant();

		if (!ValidCodes.Contains(normalized))
			throw new DomainValidationException("Currency", $"'{code}' is not a supported ISO 4217 currency code.");

		return new Currency(normalized);
	}

	public override string ToString() => Code;
}
