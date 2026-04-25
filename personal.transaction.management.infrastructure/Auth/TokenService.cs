using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using personal.transaction.management.application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace personal.transaction.management.infrastructure.Auth;

public sealed class TokenService(IOptions<JwtSettings> settings, UserContextService userContextService) : ITokenService
{
	private readonly JwtSettings settings = settings.Value;

	public string GenerateToken(Guid userId, string email, string fullName)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
			new Claim(JwtRegisteredClaimNames.Email, email),
			new Claim(JwtRegisteredClaimNames.Name, fullName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		var token = new JwtSecurityToken(
			issuer: settings.Issuer,
			audience: settings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(settings.ExpirationMinutes),
			signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
