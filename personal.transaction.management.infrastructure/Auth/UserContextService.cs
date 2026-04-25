using Microsoft.AspNetCore.Http;
using personal.transaction.management.application.Common.Interfaces;
using System.Security.Claims;

namespace personal.transaction.management.infrastructure.Auth;

public sealed class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
	public Guid UserId =>
		Guid.TryParse(httpContextAccessor.HttpContext?.User
			.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : Guid.Empty;

	public string FullName =>
		httpContextAccessor.HttpContext?.User
			.FindFirst(ClaimTypes.Name)?.Value ?? "system";
}