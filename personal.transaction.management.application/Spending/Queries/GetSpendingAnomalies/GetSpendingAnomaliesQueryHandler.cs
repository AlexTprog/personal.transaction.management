using MediatR;
using personal.transaction.management.application.Spending;
using personal.transaction.management.application.Spendings.Dtos;

namespace personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

public class GetSpendingAnomaliesQueryHandler(ISpendingRepository spendingRepository) : IRequestHandler<GetSpendingAnomaliesQuery, ICollection<SpendingAnomalyDto>>
{
	public async Task<ICollection<SpendingAnomalyDto>> Handle(GetSpendingAnomaliesQuery request, CancellationToken cancellationToken)
	{
		var to = request.From.AddMonths(-11);
		var currentCategories = await spendingRepository.GetSpendingsByCategoryAsync(request.UserId, request.From, cancellationToken);

		var averageCategories = await spendingRepository.GetSpedingAverageByCategoryAsync(request.UserId, to, request.From, cancellationToken);

		var anomalies = currentCategories.Where(x =>
		{
			var average = averageCategories.FirstOrDefault(a => a.CategoryId == x.CategoryId && a.Currency == x.Currency)?.Amount ?? 0;
			return x.Amount > average;
		}).Select(x => new SpendingAnomalyDto
		{
			CategoryId = x.CategoryId,
			CategoryName = x.CategoryName,
			Amount = x.Amount,
			Currency = x.Currency,
			Date = request.From,
			AverageAmount = averageCategories.FirstOrDefault(a => a.CategoryId == x.CategoryId && a.Currency == x.Currency)?.Amount ?? 0
		}).ToList();


		return anomalies;
	}
}
