namespace personal.transaction.management.domain.entities;

public class TransactionTag
{
	public Guid TransactionId { get; private set; }
	public Guid TagId { get; private set; }

	public Transaction Transaction { get; private set; } = null!;
	public Tag Tag { get; private set; } = null!;

	private TransactionTag() { }

	private TransactionTag(Guid transactionId, Guid tagId)
	{
		TransactionId = transactionId;
		TagId = tagId;
	}

	public static TransactionTag Create(Guid transactionId, Guid tagId)
		=> new(transactionId, tagId);
}
