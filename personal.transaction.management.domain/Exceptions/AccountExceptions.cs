namespace personal.transaction.management.domain.exceptions;

public sealed class InsufficientFundsException : DomainException
{
    public decimal Balance { get; }
    public decimal AttemptedDebit { get; }
    public string Currency { get; }

    public InsufficientFundsException(decimal balance, decimal attemptedDebit, string currency)
        : base($"Insufficient funds. Balance: {balance} {currency}, attempted debit: {attemptedDebit} {currency}.")
    {
        Balance = balance;
        AttemptedDebit = attemptedDebit;
        Currency = currency;
    }
}

public sealed class AccountCurrencyMismatchException : DomainException
{
    public string AccountCurrency { get; }
    public string TransactionCurrency { get; }

    public AccountCurrencyMismatchException(string accountCurrency, string transactionCurrency)
        : base($"Account currency '{accountCurrency}' does not match transaction currency '{transactionCurrency}'.")
    {
        AccountCurrency = accountCurrency;
        TransactionCurrency = transactionCurrency;
    }
}