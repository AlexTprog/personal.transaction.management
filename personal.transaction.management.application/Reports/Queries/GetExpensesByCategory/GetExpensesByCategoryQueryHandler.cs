using MediatR;
using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports.Queries.GetExpensesByCategory;

public class GetExpensesByCategoryQueryHandler(IReportRepository reportRepository) : IRequestHandler<GetExpensesByCategoryQuery, ExpensesByCategoryDto>
{
	public Task<ExpensesByCategoryDto> Handle(GetExpensesByCategoryQuery request, CancellationToken cancellationToken)
		=> reportRepository.GetExpensesByCategoryAsync(request.UserId, request.From, request.To, cancellationToken);
}
