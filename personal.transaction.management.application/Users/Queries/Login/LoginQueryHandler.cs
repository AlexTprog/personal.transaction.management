using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.application.Common.Interfaces;
using personal.transaction.management.application.Users.Dtos;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Users.Queries.Login;

public sealed class LoginQueryHandler(
	IUserRepository userRepository,
	IPasswordHasher passwordHasher,
	ITokenService tokenService) : IRequestHandler<LoginQuery, AuthResponseDto>
{
	public async Task<AuthResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken)
			?? throw new UnauthorizedException("Invalid email or password.");

		if (!user.IsActive)
			throw new UnauthorizedException("Account is deactivated.");

		if (!passwordHasher.Verify(request.Password, user.PasswordHash))
			throw new UnauthorizedException("Invalid email or password.");

		var token = tokenService.GenerateToken(user.Id, user.Email.Value, user.FullName);

		return new AuthResponseDto
		{
			UserId = user.Id,
			Email = user.Email.Value,
			FullName = user.FullName,
			Token = token
		};
	}
}
