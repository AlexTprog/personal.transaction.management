using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Tags.Commands.CreateTag;
using personal.transaction.management.application.Tags.Commands.DeleteTag;
using personal.transaction.management.application.Tags.Commands.RenameTag;
using personal.transaction.management.application.Tags.Dtos;
using personal.transaction.management.application.Tags.Queries.GetTagsByUser;

namespace personal.transaction.management.api.Controllers;

[Route("api/tags")]
public sealed class TagsController : BaseController
{
	[HttpGet]
	[ProducesResponseType(typeof(IReadOnlyList<TagDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetTagsByUserQuery(CurrentUserId), cancellationToken);
		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create(
		[FromBody] CreateTagRequest request,
		CancellationToken cancellationToken)
	{
		var id = await Sender.Send(new CreateTagCommand(CurrentUserId, request.Name), cancellationToken);
		return Created($"api/tags/{id}", id);
	}

	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Rename(
		Guid id,
		[FromBody] RenameTagRequest request,
		CancellationToken cancellationToken)
	{
		await Sender.Send(new RenameTagCommand(id, CurrentUserId, request.Name), cancellationToken);
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		await Sender.Send(new DeleteTagCommand(id, CurrentUserId), cancellationToken);
		return NoContent();
	}
}

public record CreateTagRequest(string Name);
public record RenameTagRequest(string Name);
