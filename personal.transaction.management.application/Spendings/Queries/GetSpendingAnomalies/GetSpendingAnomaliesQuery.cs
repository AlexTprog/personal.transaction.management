using MediatR;
using personal.transaction.management.application.Spendings.Dtos;

namespace personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

public record GetSpendingAnomaliesQuery(DateOnly From, Guid UserId) : IRequest<ICollection<SpendingAnomalyDto>>;
