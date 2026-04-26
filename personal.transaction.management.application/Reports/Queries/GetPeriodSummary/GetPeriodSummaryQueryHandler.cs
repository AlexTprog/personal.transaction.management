using MediatR;
using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports.Queries.GetPeriodSummary;

public class GetPeriodSummaryQueryHandler(IReportRepository reportRepository) : IRequestHandler<GetPeriodSummaryQuery, PeriodSummaryDto>
{
    public Task<PeriodSummaryDto> Handle(GetPeriodSummaryQuery request, CancellationToken cancellationToken)
        => reportRepository.GetPeriodSummaryAsync(request.UserId, request.From, request.To, cancellationToken);
}
