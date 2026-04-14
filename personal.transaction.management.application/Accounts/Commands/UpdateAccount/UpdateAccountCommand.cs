using MediatR;

namespace personal.transaction.management.application.Accounts.Commands.UpdateAccount;

public record UpdateAccountCommand(Guid AccountId, Guid UserId, string Name) : IRequest;
