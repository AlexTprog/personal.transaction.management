using MediatR;
using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports.Queries.GetCurrentBalance;

public record GetCurrentBalanceQuery(Guid UserId) : IRequest<CurrentBalanceDto>;
