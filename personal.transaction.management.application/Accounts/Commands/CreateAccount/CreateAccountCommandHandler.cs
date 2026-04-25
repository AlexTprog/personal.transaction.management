using MediatR;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.application.Accounts.Commands.CreateAccount;

public sealed class CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateAccountCommand, Guid>
{
	public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
	{
		var money = Money.Of(request.Amount, request.Currency);
		var account = Account.Create(
			request.UserId,
			request.Name,
			request.AccountType,
			money);

		await accountRepository.AddAsync(account, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		return account.Id;
	}
}
