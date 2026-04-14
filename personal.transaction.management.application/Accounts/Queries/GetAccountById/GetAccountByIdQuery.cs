using MediatR;
using personal.transaction.management.application.Accounts.Dtos;

namespace personal.transaction.management.application.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid AccountId, Guid UserId) : IRequest<AccountDto>;
