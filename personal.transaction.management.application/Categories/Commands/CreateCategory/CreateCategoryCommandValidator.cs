using FluentValidation;

namespace personal.transaction.management.application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	public CreateCategoryCommandValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
		RuleFor(x => x.Icon).NotEmpty().MaximumLength(100);
		RuleFor(x => x.Color).NotEmpty().Matches(@"^#[0-9A-Fa-f]{6}$")
			.WithMessage("Color must be a valid hex color (#RRGGBB).");
		RuleFor(x => x.CategoryType).IsInEnum();
	}
}
