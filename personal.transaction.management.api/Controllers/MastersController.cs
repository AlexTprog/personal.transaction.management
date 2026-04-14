using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.api.Controllers;

[ApiController]
[Route("api/masters")]
[AllowAnonymous]
public sealed class MastersController : ControllerBase
{
	[HttpGet("currencies")]
	[ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
	public IActionResult GetCurrencies() =>
		Ok(Currency.Availables.OrderBy(c => c));

	[HttpGet("account-types")]
	[ProducesResponseType(typeof(IEnumerable<MasterDto>), StatusCodes.Status200OK)]
	public IActionResult GetAccountTypes() =>
		Ok(ToLookups<AccountTypeEnum>());

	[HttpGet("category-types")]
	[ProducesResponseType(typeof(IEnumerable<MasterDto>), StatusCodes.Status200OK)]
	public IActionResult GetCategoryTypes() =>
		Ok(ToLookups<CategoryTypeEnum>());

	[HttpGet("transaction-types")]
	[ProducesResponseType(typeof(IEnumerable<MasterDto>), StatusCodes.Status200OK)]
	public IActionResult GetTransactionTypes() =>
		Ok(ToLookups<TransactionTypeEnum>());

	private static IEnumerable<MasterDto> ToLookups<TEnum>() where TEnum : struct, Enum =>
		Enum.GetValues<TEnum>()
			.Select(e => new MasterDto(Convert.ToInt32(e), e.ToString()));
}

public record MasterDto(int Value, string Name);
