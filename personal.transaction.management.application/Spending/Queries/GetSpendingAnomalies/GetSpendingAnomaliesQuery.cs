using MediatR;
using personal.transaction.management.application.Spendings.Dtos;

namespace personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

public class GetSpendingAnomaliesQuery : IRequest<ICollection<SpendingAnomalyDto>>
{
	public DateOnly From { get; init; }
	public Guid UserId { get; init; }
}
