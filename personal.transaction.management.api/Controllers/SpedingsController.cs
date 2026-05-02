using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Spendings.Dtos;
using personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

namespace personal.transaction.management.api.Controllers;

[Route("api/[controller]")]
public class SpendingsController : BaseController
{
	[HttpGet]
	[ProducesResponseType(typeof(IReadOnlyList<SpendingAnomalyDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAnomalies([FromQuery] DateOnly from, CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetSpendingAnomaliesQuery(from, CurrentUserId), cancellationToken);
		return Ok(result);
	}
}
