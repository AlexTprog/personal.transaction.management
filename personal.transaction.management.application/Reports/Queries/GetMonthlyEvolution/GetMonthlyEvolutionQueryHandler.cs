using MediatR;
using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports.Queries.GetMonthlyEvolution;

public class GetMonthlyEvolutionQueryHandler(IReportRepository reportRepository) : IRequestHandler<GetMonthlyEvolutionQuery, ICollection<MonthlyEvolutionDto>>
{
	public Task<ICollection<MonthlyEvolutionDto>> Handle(GetMonthlyEvolutionQuery request, CancellationToken cancellationToken)
	{
		var fromDate = request.Date.AddMonths(-request.LastMonths);
		return reportRepository.GetMonthlyEvolutionAsync(request.UserId, fromDate, request.Date, cancellationToken);
	}
}
