using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Of_Should_CreateMoney_When_ValueIsPositive()
    {
        var money = Money.Of(150.75m, "usd");

        Assert.Equal(150.75m, money.Value);
        Assert.Equal("USD", money.Currency.Code);
    }

    [Fact]
    public void Of_Should_CreateMoney_When_CurrencyInstanceIsProvided()
    {
        var currency = Currency.From("EUR");

        var money = Money.Of(10m, currency);

        Assert.Equal(10m, money.Value);
        Assert.Same(currency, money.Currency);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Of_Should_Throw_When_ValueIsNotPositive(decimal value)
    {
        var ex = Assert.Throws<DomainValidationException>(() => Money.Of(value, "USD"));

        Assert.Equal("Amount", ex.Field);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Of_WithCurrency_Should_Throw_When_ValueIsNotPositive(decimal value)
    {
        var ex = Assert.Throws<DomainValidationException>(() => Money.Of(value, Currency.From("USD")));

        Assert.Equal("Amount", ex.Field);
    }

    [Fact]
    public void Of_Should_Throw_When_CurrencyIsInvalid()
    {
        var ex = Assert.Throws<DomainValidationException>(() => Money.Of(10m, "XXX"));

        Assert.Equal("Currency", ex.Field);
    }

    [Fact]
    public void Money_Should_BeEqual_When_ValueAndCurrencyMatch()
    {
        Assert.Equal(Money.Of(10m, "USD"), Money.Of(10m, "usd"));
        Assert.NotEqual(Money.Of(10m, "USD"), Money.Of(10m, "EUR"));
        Assert.NotEqual(Money.Of(10m, "USD"), Money.Of(11m, "USD"));
    }

    [Fact]
    public void ToString_Should_IncludeValueAndCurrency()
    {
        Assert.Equal("25.50 PEN", Money.Of(25.50m, "PEN").ToString());
    }
}
