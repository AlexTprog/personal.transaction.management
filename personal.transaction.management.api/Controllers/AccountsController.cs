using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Accounts.Commands.CreateAccount;
using personal.transaction.management.application.Accounts.Commands.DeactivateAccount;
using personal.transaction.management.application.Accounts.Commands.UpdateAccount;
using personal.transaction.management.application.Accounts.Dtos;
using personal.transaction.management.application.Accounts.Queries.GetAccountById;
using personal.transaction.management.application.Accounts.Queries.GetAccountsByUser;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.api.Controllers;

[Route("api/accounts")]
public sealed class AccountsController : BaseController
{
	[HttpGet]
	[ProducesResponseType(typeof(IReadOnlyList<AccountDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetAccountsByUserQuery(CurrentUserId), cancellationToken);
		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetAccountByIdQuery(id, CurrentUserId), cancellationToken);
		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create(
		[FromBody] CreateAccountRequest request,
		CancellationToken cancellationToken)
	{
		var command = new CreateAccountCommand(CurrentUserId, request.Name, request.AccountType, request.Amount, request.Currency);
		var id = await Sender.Send(command, cancellationToken);
		return CreatedAtAction(nameof(GetById), new { id }, id);
	}

	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(
		Guid id,
		[FromBody] UpdateAccountRequest request,
		CancellationToken cancellationToken)
	{
		await Sender.Send(new UpdateAccountCommand(id, CurrentUserId, request.Name), cancellationToken);
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
	{
		await Sender.Send(new DeactivateAccountCommand(id, CurrentUserId), cancellationToken);
		return NoContent();
	}
}

public record CreateAccountRequest(string Name, AccountTypeEnum AccountType, string Currency, decimal Amount);
public record UpdateAccountRequest(string Name);
