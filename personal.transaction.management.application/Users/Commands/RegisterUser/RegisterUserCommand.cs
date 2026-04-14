using MediatR;
using personal.transaction.management.application.Users.Dtos;

namespace personal.transaction.management.application.Users.Commands.RegisterUser;

public record RegisterUserCommand(
	string Email,
	string FullName,
	string Password) : IRequest<AuthResponseDto>;
