using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Reports.Queries.GetCurrentBalance;
using personal.transaction.management.application.Reports.Queries.GetExpensesByCategory;
using personal.transaction.management.application.Reports.Queries.GetMonthlyEvolution;
using personal.transaction.management.application.Reports.Queries.GetPeriodSummary;

namespace personal.transaction.management.api.Controllers;

[Route("api/reports")]
public sealed class ReportsController : BaseController
{
	[HttpGet("monthly-evolution")]
	public async Task<IActionResult> GetMonthlyEvolution(
		[FromQuery] DateOnly date,
		[FromQuery] int lastMonths,
		CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetMonthlyEvolutionQuery(CurrentUserId, date, lastMonths), cancellationToken);
		return Ok(result);
	}

	[HttpGet("period-summary")]
	public async Task<IActionResult> GetPeriodSummary(
		[FromQuery] DateOnly from,
		[FromQuery] DateOnly to,
		CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetPeriodSummaryQuery(CurrentUserId, from, to), cancellationToken);
		return Ok(result);
	}

	[HttpGet("expenses-by-category")]
	public async Task<IActionResult> GetExpensesByCategory(
		[FromQuery] DateOnly from,
		[FromQuery] DateOnly to,
		CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetExpensesByCategoryQuery(CurrentUserId, from, to), cancellationToken);
		return Ok(result);
	}

	[HttpGet("current-balance")]
	public async Task<IActionResult> GetCurrentBalance(CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetCurrentBalanceQuery(CurrentUserId), cancellationToken);
		return Ok(result);
	}
}
