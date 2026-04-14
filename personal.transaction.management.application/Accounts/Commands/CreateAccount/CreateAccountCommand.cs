using MediatR;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(
	Guid UserId,
	string Name,
	AccountTypeEnum AccountType,
	decimal Amount,
	string Currency) : IRequest<Guid>;
