using MediatR;

namespace personal.transaction.management.application.Reports.Queries.GetMonthlyEvolution;

public record GetMonthlyEvolutionQuery : IRequest<GetMonthlyEvolutionQuery>
{
}
