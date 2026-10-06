using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.events;
using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.tests.Entities;

public class TransactionTests
{
    private static readonly DateOnly Today = new(2026, 10, 6);

    private static Transaction CreateTransaction(
        TransactionTypeEnum type = TransactionTypeEnum.Expense,
        Guid? transferId = null,
        decimal amount = 50m) =>
        Transaction.Create(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            Money.Of(amount, "USD"), type, "Groceries", Today,
            transferId, null, null);

    [Fact]
    public void Create_Should_InitializeTransaction_When_DataIsValid()
    {
        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var amount = Money.Of(75m, "USD");

        var transaction = Transaction.Create(
            accountId, userId, categoryId, amount, TransactionTypeEnum.Income,
            "Salary", Today, null, 1.5m, "https://files/receipt.pdf");

        Assert.NotEqual(Guid.Empty, transaction.Id);
        Assert.Equal(accountId, transaction.AccountId);
        Assert.Equal(userId, transaction.UserId);
        Assert.Equal(categoryId, transaction.CategoryId);
        Assert.Equal(amount, transaction.Amount);
        Assert.Equal(TransactionTypeEnum.Income, transaction.TransactionType);
        Assert.Equal("Salary", transaction.Description);
        Assert.Equal(Today, transaction.Date);
        Assert.Null(transaction.TransferId);
        Assert.Equal(1.5m, transaction.ExchangeRate);
        Assert.Equal("https://files/receipt.pdf", transaction.AttachmentUrl);
        Assert.Empty(transaction.Tags);
    }

    [Fact]
    public void Create_Should_RaiseTransactionCreatedEvent()
    {
        var transaction = CreateTransaction(TransactionTypeEnum.Expense, amount: 20m);

        var evt = Assert.IsType<TransactionCreatedEvent>(Assert.Single(transaction.DomainEvents));
        Assert.Equal(transaction.Id, evt.TransactionId);
        Assert.Equal(transaction.AccountId, evt.AccountId);
        Assert.Equal(transaction.UserId, evt.UserId);
        Assert.Equal(Money.Of(20m, "USD"), evt.Amount);
        Assert.Equal(TransactionTypeEnum.Expense, evt.TransactionType);
    }

    [Theory]
    [InlineData(TransactionTypeEnum.TransferIn)]
    [InlineData(TransactionTypeEnum.TransferOut)]
    public void Create_Should_Throw_When_TransferHasNoTransferId(TransactionTypeEnum type)
    {
        Assert.Throws<TransferIdRequiredException>(() => CreateTransaction(type, transferId: null));
    }

    [Theory]
    [InlineData(TransactionTypeEnum.Income)]
    [InlineData(TransactionTypeEnum.Expense)]
    public void Create_Should_Throw_When_NonTransferHasTransferId(TransactionTypeEnum type)
    {
        Assert.Throws<TransferIdForbiddenException>(() => CreateTransaction(type, transferId: Guid.NewGuid()));
    }

    [Theory]
    [InlineData(TransactionTypeEnum.TransferIn)]
    [InlineData(TransactionTypeEnum.TransferOut)]
    public void Create_Should_Succeed_When_TransferHasTransferId(TransactionTypeEnum type)
    {
        var transferId = Guid.NewGuid();

        var transaction = CreateTransaction(type, transferId);

        Assert.Equal(transferId, transaction.TransferId);
    }

    [Fact]
    public void Update_Should_ChangeFieldsAndRaiseUpdatedEvent()
    {
        var transaction = CreateTransaction(amount: 50m);
        transaction.ClearDomainEvents();
        var newCategoryId = Guid.NewGuid();
        var newDate = Today.AddDays(-1);

        transaction.Update(newCategoryId, Money.Of(80m, "USD"), "Dinner", newDate, "https://files/a.png");

        Assert.Equal(newCategoryId, transaction.CategoryId);
        Assert.Equal(Money.Of(80m, "USD"), transaction.Amount);
        Assert.Equal("Dinner", transaction.Description);
        Assert.Equal(newDate, transaction.Date);
        Assert.Equal("https://files/a.png", transaction.AttachmentUrl);

        var evt = Assert.IsType<TransactionUpdatedEvent>(Assert.Single(transaction.DomainEvents));
        Assert.Equal(transaction.Id, evt.TransactionId);
        Assert.Equal(transaction.AccountId, evt.AccountId);
        Assert.Equal(Money.Of(50m, "USD"), evt.PreviousAmount);
        Assert.Equal(Money.Of(80m, "USD"), evt.NewAmount);
        Assert.Equal(TransactionTypeEnum.Expense, evt.TransactionType);
    }

    [Theory]
    [InlineData(TransactionTypeEnum.TransferIn)]
    [InlineData(TransactionTypeEnum.TransferOut)]
    public void Update_Should_Throw_When_TransactionIsTransfer(TransactionTypeEnum type)
    {
        var transaction = CreateTransaction(type, Guid.NewGuid(), 50m);
        transaction.ClearDomainEvents();

        Assert.Throws<TransferPartialModificationException>(() =>
            transaction.Update(Guid.NewGuid(), Money.Of(10m, "USD"), null, Today, null));

        Assert.Equal(Money.Of(50m, "USD"), transaction.Amount);
        Assert.Empty(transaction.DomainEvents);
    }

    [Fact]
    public void Delete_Should_RaiseTransactionDeletedEvent()
    {
        var transaction = CreateTransaction(TransactionTypeEnum.Income, amount: 30m);
        transaction.ClearDomainEvents();

        transaction.Delete();

        var evt = Assert.IsType<TransactionDeletedEvent>(Assert.Single(transaction.DomainEvents));
        Assert.Equal(transaction.Id, evt.TransactionId);
        Assert.Equal(transaction.AccountId, evt.AccountId);
        Assert.Equal(Money.Of(30m, "USD"), evt.Amount);
        Assert.Equal(TransactionTypeEnum.Income, evt.TransactionType);
    }

    [Fact]
    public void AddTag_Should_AddTransactionTag()
    {
        var transaction = CreateTransaction();
        var tag = Tag.CreateUserTag(transaction.UserId, "food");

        transaction.AddTag(tag);

        var entry = Assert.Single(transaction.Tags);
        Assert.Equal(transaction.Id, entry.TransactionId);
        Assert.Equal(tag.Id, entry.TagId);
    }

    [Fact]
    public void AddTag_Should_BeIdempotent_When_TagAlreadyAdded()
    {
        var transaction = CreateTransaction();
        var tag = Tag.CreateUserTag(transaction.UserId, "food");

        transaction.AddTag(tag);
        transaction.AddTag(tag);

        Assert.Single(transaction.Tags);
    }

    [Fact]
    public void RemoveTag_Should_RemoveExistingTag()
    {
        var transaction = CreateTransaction();
        var food = Tag.CreateUserTag(transaction.UserId, "food");
        var travel = Tag.CreateUserTag(transaction.UserId, "travel");
        transaction.AddTag(food);
        transaction.AddTag(travel);

        transaction.RemoveTag(food.Id);

        var remaining = Assert.Single(transaction.Tags);
        Assert.Equal(travel.Id, remaining.TagId);
    }

    [Fact]
    public void RemoveTag_Should_DoNothing_When_TagNotPresent()
    {
        var transaction = CreateTransaction();
        transaction.AddTag(Tag.CreateUserTag(transaction.UserId, "food"));

        transaction.RemoveTag(Guid.NewGuid());

        Assert.Single(transaction.Tags);
    }

    [Fact]
    public void ClearDomainEvents_Should_RemoveAllEvents()
    {
        var transaction = CreateTransaction();
        transaction.Delete();

        transaction.ClearDomainEvents();

        Assert.Empty(transaction.DomainEvents);
    }
}
