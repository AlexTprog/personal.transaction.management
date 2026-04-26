
using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Reports.Queries.GetMonthlyEvolution;

namespace personal.transaction.management.api.Controllers;


[Route("api/reports")]
public sealed class ReportsController : BaseController
{
	[HttpGet("monthly-evolution")]
	public async Task<IActionResult> GetReport(
		[FromQuery] DateOnly date,
		[FromQuery] int lastMonths,
		CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetMonthlyEvolutionQuery(CurrentUserId, date, lastMonths), cancellationToken);
		return Ok(result);
	}
}
