using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace personal.transaction.management.api.Controllers;

[ApiController]
[Authorize]
public abstract class BaseController : ControllerBase
{
	private ISender? _sender;

	protected ISender Sender =>
		_sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

	protected Guid CurrentUserId
	{
		get
		{
			var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
				?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

			return Guid.TryParse(value, out var id)
				? id
				: throw new InvalidOperationException("User ID claim is missing or invalid.");
		}
	}
}
