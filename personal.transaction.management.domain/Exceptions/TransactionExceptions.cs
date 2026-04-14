namespace personal.transaction.management.domain.exceptions;

public sealed class FutureDateTransactionException : DomainException
{
	public FutureDateTransactionException()
		: base("Transaction date cannot be in the future.") { }
}

public sealed class TransferIdRequiredException : DomainException
{
	public TransferIdRequiredException()
		: base("TransferId is required for transfer transactions.") { }
}

public sealed class TransferIdForbiddenException : DomainException
{
	public TransferIdForbiddenException()
		: base("TransferId must be null for non-transfer transactions.") { }
}

public sealed class TransferPartialModificationException : DomainException
{
	public TransferPartialModificationException()
		: base("Transfer transactions cannot be partially modified. Delete and recreate the transfer.") { }
}
