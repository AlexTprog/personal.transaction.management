namespace personal.transaction.management.application.Users.Dtos;

public record AuthResponseDto
{
	public required Guid UserId { get; init; }
	public required string Email { get; init; }
	public required string FullName { get; init; }
	public required string Token { get; init; }
}
