using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.application.Common.Interfaces;
using personal.transaction.management.application.Users.Dtos;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(
	IUserRepository userRepository,
	IUnitOfWork unitOfWork,
	IPasswordHasher passwordHasher,
	ITokenService tokenService) : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
	public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var emailExists = await userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
		if (emailExists)
			throw new ConflictException($"A user with email '{request.Email}' already exists.");

		var passwordHash = passwordHasher.Hash(request.Password);
		var user = User.Create(request.Email, request.FullName, passwordHash);

		await userRepository.AddAsync(user, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

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
