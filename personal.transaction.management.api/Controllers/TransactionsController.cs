using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Common;
using personal.transaction.management.application.Transactions.Commands.CreateTransaction;
using personal.transaction.management.application.Transactions.Commands.CreateTransfer;
using personal.transaction.management.application.Transactions.Commands.DeleteTransaction;
using personal.transaction.management.application.Transactions.Commands.UpdateTransaction;
using personal.transaction.management.application.Transactions.Dtos;
using personal.transaction.management.application.Transactions.Queries.GetPagedTransactions;
using personal.transaction.management.application.Transactions.Queries.GetTransactionById;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.api.Controllers;

[Route("api/transactions")]
public sealed class TransactionsController : BaseController
{
	[HttpGet]
	[ProducesResponseType(typeof(PagedResult<TransactionDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetPaged(
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var result = await Sender.Send(
			new GetPagedTransactionsQuery(CurrentUserId, page, pageSize),
			cancellationToken);
		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetTransactionByIdQuery(id, CurrentUserId), cancellationToken);
		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	public async Task<IActionResult> Create(
		[FromBody] CreateTransactionRequest request,
		CancellationToken cancellationToken)
	{
		var command = new CreateTransactionCommand(
			request.AccountId,
			CurrentUserId,
			request.CategoryId,
			request.Amount,
			request.Currency,
			request.TransactionType,
			request.Description,
			request.Date,
			request.ExchangeRate,
			request.AttachmentUrl,
			request.TagIds);

		var id = await Sender.Send(command, cancellationToken);
		return CreatedAtAction(nameof(GetById), new { id }, id);
	}

	[HttpPost("transfer")]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	public async Task<IActionResult> CreateTransfer(
		[FromBody] CreateTransferRequest request,
		CancellationToken cancellationToken)
	{
		var command = new CreateTransferCommand(
			CurrentUserId,
			request.SourceAccountId,
			request.DestinationAccountId,
			request.CategoryId,
			request.Amount,
			request.Description,
			request.Date,
			request.ExchangeRate);

		var transferId = await Sender.Send(command, cancellationToken);
		return CreatedAtAction(nameof(GetById), new { id = transferId }, transferId);
	}

	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	public async Task<IActionResult> Update(
		Guid id,
		[FromBody] UpdateTransactionRequest request,
		CancellationToken cancellationToken)
	{
		var command = new UpdateTransactionCommand(
			id,
			CurrentUserId,
			request.CategoryId,
			request.Amount,
			request.Currency,
			request.Description,
			request.Date,
			request.AttachmentUrl);

		await Sender.Send(command, cancellationToken);
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		await Sender.Send(new DeleteTransactionCommand(id, CurrentUserId), cancellationToken);
		return NoContent();
	}
}

public record CreateTransactionRequest(
	Guid AccountId,
	Guid CategoryId,
	decimal Amount,
	string Currency,
	TransactionTypeEnum TransactionType,
	string? Description,
	DateOnly Date,
	decimal? ExchangeRate,
	string? AttachmentUrl,
	List<Guid>? TagIds);

public record CreateTransferRequest(
	Guid SourceAccountId,
	Guid DestinationAccountId,
	Guid CategoryId,
	decimal Amount,
	string? Description,
	DateOnly Date,
	decimal? ExchangeRate);

public record UpdateTransactionRequest(
	Guid CategoryId,
	decimal Amount,
	string Currency,
	string? Description,
	DateOnly Date,
	string? AttachmentUrl);
