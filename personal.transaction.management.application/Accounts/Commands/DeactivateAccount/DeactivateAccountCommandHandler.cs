using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Accounts.Commands.DeactivateAccount;

public sealed class DeactivateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeactivateAccountCommand>
{
	public async Task Handle(DeactivateAccountCommand request, CancellationToken cancellationToken)
	{
		var account = await accountRepository.GetByIdAndUserIdAsync(
			request.AccountId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Account), request.AccountId);

		account.Deactivate();
		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
