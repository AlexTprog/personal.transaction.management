using MediatR;

namespace personal.transaction.management.application.Accounts.Commands.DeactivateAccount;

public record DeactivateAccountCommand(Guid AccountId, Guid UserId) : IRequest;
