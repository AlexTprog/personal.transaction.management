using MediatR;
using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports.Queries.GetPeriodSummary;

public record GetPeriodSummaryQuery(Guid UserId, DateOnly From, DateOnly To) : IRequest<PeriodSummaryDto>;
