using FluentValidation;

namespace personal.transaction.management.application.Tags.Commands.RenameTag;

public sealed class RenameTagCommandValidator : AbstractValidator<RenameTagCommand>
{
	public RenameTagCommandValidator()
	{
		RuleFor(x => x.TagId).NotEmpty();
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
	}
}
