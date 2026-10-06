using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.tests.Entities;

public class AccountTests
{
    private static Account CreateAccount(decimal balance = 100m, string currency = "USD") =>
        Account.Create(Guid.NewGuid(), "Main", AccountTypeEnum.Bank, Money.Of(balance, currency));

    [Fact]
    public void Create_Should_InitializeAccount_When_DataIsValid()
    {
        var userId = Guid.NewGuid();

        var account = Account.Create(userId, "  Savings  ", AccountTypeEnum.Cash, Money.Of(250m, "EUR"));

        Assert.NotEqual(Guid.Empty, account.Id);
        Assert.Equal(userId, account.UserId);
        Assert.Equal("Savings", account.Name);
        Assert.Equal(AccountTypeEnum.Cash, account.AccountType);
        Assert.Equal("EUR", account.Currency.Code);
        Assert.Equal(250m, account.Balance);
        Assert.True(account.IsActive);
        Assert.Empty(account.DomainEvents);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_Should_Throw_When_NameIsEmpty(string? name)
    {
        var ex = Assert.Throws<DomainValidationException>(() =>
            Account.Create(Guid.NewGuid(), name!, AccountTypeEnum.Bank, Money.Of(1m, "USD")));

        Assert.Equal("Name", ex.Field);
    }

    [Fact]
    public void Credit_Should_IncreaseBalance_When_CurrencyMatches()
    {
        var account = CreateAccount(100m);

        account.Credit(Money.Of(50.25m, "usd"));

        Assert.Equal(150.25m, account.Balance);
    }

    [Fact]
    public void Credit_Should_Throw_When_CurrencyDiffers()
    {
        var account = CreateAccount(100m, "USD");

        var ex = Assert.Throws<AccountCurrencyMismatchException>(() => account.Credit(Money.Of(10m, "EUR")));

        Assert.Equal("USD", ex.AccountCurrency);
        Assert.Equal("EUR", ex.TransactionCurrency);
        Assert.Equal(100m, account.Balance);
    }

    [Fact]
    public void Debit_Should_DecreaseBalance_When_FundsAreSufficient()
    {
        var account = CreateAccount(100m);

        account.Debit(Money.Of(40m, "USD"));

        Assert.Equal(60m, account.Balance);
    }

    [Fact]
    public void Debit_Should_AllowZeroBalance_When_DebitEqualsBalance()
    {
        var account = CreateAccount(100m);

        account.Debit(Money.Of(100m, "USD"));

        Assert.Equal(0m, account.Balance);
    }

    [Fact]
    public void Debit_Should_Throw_When_FundsAreInsufficient()
    {
        var account = CreateAccount(100m);

        var ex = Assert.Throws<InsufficientFundsException>(() => account.Debit(Money.Of(100.01m, "USD")));

        Assert.Equal(100m, ex.Balance);
        Assert.Equal(100.01m, ex.AttemptedDebit);
        Assert.Equal("USD", ex.Currency);
        Assert.Equal(100m, account.Balance);
    }

    [Fact]
    public void Debit_Should_Throw_When_CurrencyDiffers()
    {
        var account = CreateAccount(100m, "USD");

        Assert.Throws<AccountCurrencyMismatchException>(() => account.Debit(Money.Of(10m, "PEN")));
        Assert.Equal(100m, account.Balance);
    }

    [Fact]
    public void Debit_Should_CheckCurrencyBeforeFunds()
    {
        var account = CreateAccount(10m, "USD");

        Assert.Throws<AccountCurrencyMismatchException>(() => account.Debit(Money.Of(1000m, "EUR")));
    }

    [Fact]
    public void Rename_Should_UpdateTrimmedName()
    {
        var account = CreateAccount();

        account.Rename("  Wallet ");

        Assert.Equal("Wallet", account.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Rename_Should_Throw_When_NameIsEmpty(string name)
    {
        var account = CreateAccount();

        Assert.Throws<DomainValidationException>(() => account.Rename(name));
        Assert.Equal("Main", account.Name);
    }

    [Fact]
    public void Deactivate_Should_SetIsActiveToFalse()
    {
        var account = CreateAccount();

        account.Deactivate();

        Assert.False(account.IsActive);
    }
}
