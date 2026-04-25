using personal.transaction.management.domain.abstractions;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.events;
using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.entities;

public class Transaction : BaseAuditable
{
	public Guid Id { get; private set; }
	public Guid AccountId { get; private set; }
	public Guid UserId { get; private set; }
	public Guid CategoryId { get; private set; }
	public Money Amount { get; private set; } = null!;
	public TransactionTypeEnum TransactionType { get; private set; }
	public string? Description { get; private set; }
	public DateOnly Date { get; private set; }
	public Guid? TransferId { get; private set; }
	public decimal? ExchangeRate { get; private set; }
	public string? AttachmentUrl { get; private set; }

	private readonly List<TransactionTag> _tags = [];
	public IReadOnlyList<TransactionTag> Tags => _tags.AsReadOnly();

	private Transaction() { }

	private Transaction(
		Guid accountId, Guid userId, Guid categoryId,
		Money amount, TransactionTypeEnum transactionType,
		string? description, DateOnly date,
		Guid? transferId, decimal? exchangeRate,
		string? attachmentUrl)
	{
		Id = Guid.NewGuid();
		AccountId = accountId;
		UserId = userId;
		CategoryId = categoryId;
		Amount = amount;
		TransactionType = transactionType;
		Description = description;
		Date = date;
		TransferId = transferId;
		ExchangeRate = exchangeRate;
		AttachmentUrl = attachmentUrl;
	}

	public static Transaction Create(
		Guid accountId, Guid userId, Guid categoryId,
		Money amount, TransactionTypeEnum transactionType,
		string? description, DateOnly date,
		Guid? transferId, decimal? exchangeRate,
		string? attachmentUrl)
	{
		if (date > DateOnly.FromDateTime(DateTime.UtcNow))
			throw new FutureDateTransactionException();

		var isTransfer = transactionType is TransactionTypeEnum.TransferIn or TransactionTypeEnum.TransferOut;

		if (isTransfer && transferId is null)
			throw new TransferIdRequiredException();

		if (!isTransfer && transferId is not null)
			throw new TransferIdForbiddenException();

		var transaction = new Transaction(
			accountId, userId, categoryId, amount, transactionType,
			description, date, transferId, exchangeRate, attachmentUrl);

		transaction.RaiseDomainEvent(new TransactionCreatedEvent(
			transaction.Id, accountId, userId, amount, transactionType));

		return transaction;
	}

	public void Update(
		Guid categoryId, Money amount, string? description,
		DateOnly date, string? attachmentUrl)
	{
		if (TransactionType is TransactionTypeEnum.TransferIn or TransactionTypeEnum.TransferOut)
			throw new TransferPartialModificationException();

		if (date > DateOnly.FromDateTime(DateTime.UtcNow))
			throw new FutureDateTransactionException();

		var previousAmount = Amount;
		CategoryId = categoryId;
		Amount = amount;
		Description = description;
		Date = date;
		AttachmentUrl = attachmentUrl;

		RaiseDomainEvent(new TransactionUpdatedEvent(Id, AccountId, previousAmount, Amount, TransactionType));
	}

	public void AddTag(Tag tag)
	{
		if (_tags.Any(t => t.TagId == tag.Id))
			return;

		_tags.Add(TransactionTag.Create(Id, tag.Id));
	}

	public void RemoveTag(Guid tagId)
	{
		var entry = _tags.FirstOrDefault(t => t.TagId == tagId);
		if (entry is not null)
			_tags.Remove(entry);
	}

	public void Delete()
	{
		RaiseDomainEvent(new TransactionDeletedEvent(Id, AccountId, Amount, TransactionType));
	}
}
