using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Categories.Commands.CreateCategory;
using personal.transaction.management.application.Categories.Commands.DeactivateCategory;
using personal.transaction.management.application.Categories.Commands.UpdateCategory;
using personal.transaction.management.application.Categories.Dtos;
using personal.transaction.management.application.Categories.Queries.GetCategoriesByUser;
using personal.transaction.management.domain.enums;

namespace personal.transaction.management.api.Controllers;

[Route("api/categories")]
public sealed class CategoriesController : BaseController
{
	[HttpGet]
	[ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var result = await Sender.Send(new GetCategoriesByUserQuery(CurrentUserId), cancellationToken);
		return Ok(result);
	}

	[HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create(
		[FromBody] CreateCategoryRequest request,
		CancellationToken cancellationToken)
	{
		var command = new CreateCategoryCommand(
			CurrentUserId,
			request.Name,
			request.Icon,
			request.Color,
			request.CategoryType);

		var id = await Sender.Send(command, cancellationToken);
		return Created($"api/categories/{id}", id);
	}

	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(
		Guid id,
		[FromBody] UpdateCategoryRequest request,
		CancellationToken cancellationToken)
	{
		var command = new UpdateCategoryCommand(
			id,
			CurrentUserId,
			request.Name,
			request.Icon,
			request.Color,
			request.CategoryType);

		await Sender.Send(command, cancellationToken);
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
	{
		await Sender.Send(new DeactivateCategoryCommand(id, CurrentUserId), cancellationToken);
		return NoContent();
	}
}

public record CreateCategoryRequest(string Name, string Icon, string Color, CategoryTypeEnum CategoryType);
public record UpdateCategoryRequest(string Name, string Icon, string Color, CategoryTypeEnum CategoryType);
