using personal.transaction.management.application.Abstractions;

namespace personal.transaction.management.application.Transactions.Commands.CreateTransfer;

public record CreateTransferCommand(
    Guid RequestId,
    Guid UserId,
    Guid SourceAccountId,
    Guid DestinationAccountId,
    Guid CategoryId,
    decimal Amount,
    string? Description,
    DateOnly Date,
    decimal? ExchangeRate) : IdempotentCommand<Guid>(RequestId);
