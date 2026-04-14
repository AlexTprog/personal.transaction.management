namespace personal.transaction.management.domain.entities;

public class TransactionTag
{
	public Guid TransactionId { get; private set; }
	public Guid TagId { get; private set; }

	// Navigation properties — populated by EF Core
	public Transaction Transaction { get; private set; } = null!;
	public Tag Tag { get; private set; } = null!;

	// Required by EF Core
	private TransactionTag() { }

	private TransactionTag(Guid transactionId, Guid tagId)
	{
		TransactionId = transactionId;
		TagId = tagId;
	}

	public static TransactionTag Create(Guid transactionId, Guid tagId)
		=> new(transactionId, tagId);
}
