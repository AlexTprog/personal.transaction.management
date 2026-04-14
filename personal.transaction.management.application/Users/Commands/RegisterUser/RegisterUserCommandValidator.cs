using FluentValidation;

namespace personal.transaction.management.application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserCommandValidator()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
		RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
		RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(100);
	}
}
