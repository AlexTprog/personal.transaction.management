using personal.transaction.management.domain.exceptions;
using System.Text.RegularExpressions;

namespace personal.transaction.management.domain.valueobjects;

public sealed record Email
{
	private static readonly Regex EmailRegex = new(
		@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase,
		TimeSpan.FromMilliseconds(100));

	public string Value { get; }

	private Email(string value) => Value = value;

	public static Email From(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			throw new DomainValidationException("Email", "Email cannot be empty.");

		var normalized = email.Trim().ToLowerInvariant();

		if (!EmailRegex.IsMatch(normalized))
			throw new DomainValidationException("Email", $"'{email}' is not a valid email address.");

		return new Email(normalized);
	}

	public override string ToString() => Value;

	public static implicit operator string(Email email) => email.Value;
}
