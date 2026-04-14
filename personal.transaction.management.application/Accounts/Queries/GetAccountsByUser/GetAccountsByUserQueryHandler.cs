using MediatR;
using personal.transaction.management.application.Accounts.Dtos;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Accounts.Queries.GetAccountsByUser;

public sealed class GetAccountsByUserQueryHandler(IAccountRepository accountRepository) : IRequestHandler<GetAccountsByUserQuery, IReadOnlyList<AccountDto>>
{
	public async Task<IReadOnlyList<AccountDto>> Handle(
		GetAccountsByUserQuery request, CancellationToken cancellationToken)
	{
		var accounts = await accountRepository.GetByUserIdAsync(request.UserId, cancellationToken);

		return accounts.Select(AccountDto.FromEntity).ToList();
	}
}
