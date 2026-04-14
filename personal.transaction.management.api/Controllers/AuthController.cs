using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Users.Commands.RegisterUser;
using personal.transaction.management.application.Users.Dtos;
using personal.transaction.management.application.Users.Queries.Login;

namespace personal.transaction.management.api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public sealed class AuthController(ISender sender) : ControllerBase
{
	[HttpPost("register")]
	[ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Register(
		[FromBody] RegisterRequest request,
		CancellationToken cancellationToken)
	{
		var command = new RegisterUserCommand(request.Email, request.FullName, request.Password);
		var response = await sender.Send(command, cancellationToken);
		return CreatedAtAction(nameof(Register), response);
	}

	[HttpPost("login")]
	[ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Login(
		[FromBody] LoginRequest request,
		CancellationToken cancellationToken)
	{
		var query = new LoginQuery(request.Email, request.Password);
		var response = await sender.Send(query, cancellationToken);
		return Ok(response);
	}
}

public record RegisterRequest(string Email, string FullName, string Password);
public record LoginRequest(string Email, string Password);
