using MediatR;
using personal.transaction.management.application.Accounts.Dtos;

namespace personal.transaction.management.application.Accounts.Queries.GetAccountsByUser;

public record GetAccountsByUserQuery(Guid UserId) : IRequest<IReadOnlyList<AccountDto>>;
