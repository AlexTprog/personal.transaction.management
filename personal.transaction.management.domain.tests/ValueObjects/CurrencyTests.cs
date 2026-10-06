using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.tests.ValueObjects;

public class CurrencyTests
{
    [Theory]
    [InlineData("USD", "USD")]
    [InlineData("usd", "USD")]
    [InlineData("  eur  ", "EUR")]
    [InlineData("Pen", "PEN")]
    public void From_Should_NormalizeCode_When_CodeIsSupported(string input, string expected)
    {
        var currency = Currency.From(input);

        Assert.Equal(expected, currency.Code);
        Assert.Equal(expected, currency.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void From_Should_Throw_When_CodeIsEmpty(string? input)
    {
        var ex = Assert.Throws<DomainValidationException>(() => Currency.From(input!));

        Assert.Equal("Currency", ex.Field);
    }

    [Theory]
    [InlineData("XXX")]
    [InlineData("US")]
    [InlineData("DOLLAR")]
    public void From_Should_Throw_When_CodeIsNotSupported(string input)
    {
        var ex = Assert.Throws<DomainValidationException>(() => Currency.From(input));

        Assert.Equal("Currency", ex.Field);
        Assert.Contains(input, ex.Message);
    }

    [Fact]
    public void Currencies_Should_BeEqual_When_CodesMatchIgnoringCase()
    {
        Assert.Equal(Currency.From("usd"), Currency.From("USD"));
    }

    [Fact]
    public void Availables_Should_ContainSupportedCodes()
    {
        Assert.Contains("USD", Currency.Availables);
        Assert.Contains("EUR", Currency.Availables);
    }
}
