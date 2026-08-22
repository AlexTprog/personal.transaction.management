using Microsoft.AspNetCore.Mvc;


namespace personal.transaction.management.api.Controllers;


[Route("api/budget")]
[ApiController]
public sealed class BudgetController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        return Ok();
    }
}
