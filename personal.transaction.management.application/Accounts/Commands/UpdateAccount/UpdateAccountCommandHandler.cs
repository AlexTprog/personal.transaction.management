using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Accounts.Commands.UpdateAccount;

public sealed class UpdateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateAccountCommand>
{
	public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
	{
		var account = await accountRepository.GetByIdAndUserIdAsync(
			request.AccountId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Account), request.AccountId);

		account.Rename(request.Name);
		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
