using MediatR;
using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports.Queries.GetCurrentBalance;

public class GetCurrentBalanceQueryHandler(IReportRepository reportRepository) : IRequestHandler<GetCurrentBalanceQuery, CurrentBalanceDto>
{
	public Task<CurrentBalanceDto> Handle(GetCurrentBalanceQuery request, CancellationToken cancellationToken)
		=> reportRepository.GetCurrentBalanceAsync(request.UserId, cancellationToken);
}
