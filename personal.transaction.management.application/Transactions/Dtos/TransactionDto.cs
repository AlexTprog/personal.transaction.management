using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.application.Transactions.Dtos;

public record TransactionDto
{
	public Guid Id { get; init; }
	public Guid AccountId { get; init; }
	public Guid UserId { get; init; }
	public Guid CategoryId { get; init; }
	public decimal Amount { get; init; }
	public string Currency { get; init; } = string.Empty;
	public TransactionTypeEnum TransactionType { get; init; }
	public string? Description { get; init; }
	public DateOnly Date { get; init; }
	public Guid? TransferId { get; init; }
	public decimal? ExchangeRate { get; init; }
	public string? AttachmentUrl { get; init; }
	public IReadOnlyList<Guid> TagIds { get; init; } = [];
	public DateTime CreatedAt { get; init; }

	public static TransactionDto FromEntity(Transaction t) => new()
	{
		Id = t.Id,
		AccountId = t.AccountId,
		UserId = t.UserId,
		CategoryId = t.CategoryId,
		Amount = t.Amount.Value,
		Currency = t.Amount.Currency.Code,
		TransactionType = t.TransactionType,
		Description = t.Description,
		Date = t.Date,
		TransferId = t.TransferId,
		ExchangeRate = t.ExchangeRate,
		AttachmentUrl = t.AttachmentUrl,
		TagIds = t.Tags.Select(tt => tt.TagId).ToList(),
		CreatedAt = t.CreatedAt
	};
}
