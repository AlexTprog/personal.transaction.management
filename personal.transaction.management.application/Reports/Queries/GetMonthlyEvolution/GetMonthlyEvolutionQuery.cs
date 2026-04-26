using MediatR;
using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports.Queries.GetMonthlyEvolution;

public record GetMonthlyEvolutionQuery(Guid UserId, DateOnly Date, int LastMonths) : IRequest<ICollection<MonthlyEvolutionDto>>;
