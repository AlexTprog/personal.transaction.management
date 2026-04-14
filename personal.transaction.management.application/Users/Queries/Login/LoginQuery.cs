using MediatR;
using personal.transaction.management.application.Users.Dtos;

namespace personal.transaction.management.application.Users.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<AuthResponseDto>;
