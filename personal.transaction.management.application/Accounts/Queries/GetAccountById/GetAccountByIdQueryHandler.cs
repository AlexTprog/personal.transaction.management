using MediatR;
using personal.transaction.management.application.Accounts.Dtos;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Accounts.Queries.GetAccountById;

public sealed class GetAccountByIdQueryHandler(IAccountRepository accountRepository) : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
	public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
	{
		var account = await accountRepository.GetByIdAndUserIdAsync(
			request.AccountId, request.UserId, cancellationToken)
			?? throw new NotFoundException(nameof(Account), request.AccountId);

		return AccountDto.FromEntity(account);
	}
}
