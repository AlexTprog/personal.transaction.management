using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using personal.transaction.management.api.Extensions;
using personal.transaction.management.api.Middleware;
using personal.transaction.management.application;
using personal.transaction.management.infrastructure;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		var jwtSection = builder.Configuration.GetSection("Jwt");
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSection["Issuer"],
			ValidAudience = jwtSection["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(jwtSection["Key"]!)),
			ClockSkew = TimeSpan.Zero
		};
	});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
	options.AddDocumentTransformer<ScalarDocumentTransformer>();
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
	options.AddPreferredSecuritySchemes("Bearer");
});

// Redirect root → Scalar UI
app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
