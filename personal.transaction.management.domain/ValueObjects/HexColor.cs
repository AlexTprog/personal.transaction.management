using personal.transaction.management.domain.exceptions;
using System.Text.RegularExpressions;

namespace personal.transaction.management.domain.valueobjects;

public sealed record HexColor
{
	private static readonly Regex HexColorRegex = new(
		@"^#[0-9A-Fa-f]{6}$",
		RegexOptions.Compiled,
		TimeSpan.FromMilliseconds(100));

	public string Value { get; }

	private HexColor(string value) => Value = value;

	public static HexColor From(string color)
	{
		if (string.IsNullOrWhiteSpace(color))
			throw new DomainValidationException("Color", "Color cannot be empty.");

		if (!HexColorRegex.IsMatch(color))
			throw new DomainValidationException("Color", $"'{color}' is not a valid hex color. Expected format: #RRGGBB.");

		return new HexColor(color.ToUpperInvariant());
	}

	public override string ToString() => Value;
}
