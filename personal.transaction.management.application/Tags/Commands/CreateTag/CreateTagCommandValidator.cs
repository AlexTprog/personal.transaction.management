using FluentValidation;

namespace personal.transaction.management.application.Tags.Commands.CreateTag;

public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
	public CreateTagCommandValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
	}
}
