using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Spendings.Queries.GetSpendingAnomalies;

namespace personal.transaction.management.api.Controllers;

[Route("api/spending")]
public sealed class SpendingController : BaseController
{
    [HttpGet("anomalies")]
    public async Task<IActionResult> GetAnomalies(
        [FromQuery] DateOnly from,
        [FromQuery] int previousMonths,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetSpendingAnomaliesQuery(from, previousMonths, CurrentUserId), cancellationToken);
        return Ok(result);
    }
}
