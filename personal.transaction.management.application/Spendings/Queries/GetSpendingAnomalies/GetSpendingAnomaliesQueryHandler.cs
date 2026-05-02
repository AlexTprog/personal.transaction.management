using MediatR;
using personal.transaction.management.application.Spendings.Dtos;

namespace personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

public class GetSpendingAnomaliesQueryHandler(ISpendingRepository spendingRepository) : IRequestHandler<GetSpendingAnomaliesQuery, ICollection<SpendingAnomalyDto>>
{
	public async Task<ICollection<SpendingAnomalyDto>> Handle(GetSpendingAnomaliesQuery request, CancellationToken cancellationToken)
	{
		var from = new DateOnly(request.From.Year, request.From.Month, 1);
		var to = from.AddMonths(-11);
		to = new DateOnly(to.Year, to.Month, DateTime.DaysInMonth(to.Year, to.Month));

		var previousMonth = from.AddMonths(-1);
		var fromPreviousMonth = new DateOnly(previousMonth.Year, previousMonth.Month, DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month));

		const decimal percentage = 0.4m;

		var currentCategories = await spendingRepository.GetSpendingByCategoriesAsync(request.UserId, from, cancellationToken);
		var averageCategories = await spendingRepository.GetSpendingAverageByCategoriesAsync(request.UserId, to, fromPreviousMonth, cancellationToken);

		var anomalies = currentCategories
		.Select(x =>
		{
			var average = averageCategories.FirstOrDefault(a => a.CategoryId == x.CategoryId && a.Currency == x.Currency)?.Amount ?? 0;
			var deviationFactor = average != 0 ? x.Amount / average : 0;
			return new SpendingAnomalyDto
			{
				CategoryId = x.CategoryId,
				CategoryName = x.CategoryName,
				Amount = x.Amount,
				Currency = x.Currency,
				Date = request.From,
				AverageAmount = average,
				DeviationFactor = deviationFactor,
			};
		})
		.Where(x => x.DeviationFactor > (1 + percentage))
		.ToList();

		return anomalies;
	}
}
