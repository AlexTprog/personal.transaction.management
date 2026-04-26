using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using personal.transaction.management.application.Common.Interfaces;
using personal.transaction.management.application.Reports;
using personal.transaction.management.domain.repositories;
using personal.transaction.management.infrastructure.Auth;
using personal.transaction.management.infrastructure.Persistence;
using personal.transaction.management.infrastructure.Persistence.Repositories;

namespace personal.transaction.management.infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(connectionString));

		services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IAccountRepository, AccountRepository>();
		services.AddScoped<ITransactionRepository, TransactionRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<ITagRepository, TagRepository>();
		services.AddScoped<IReportRepository, ReportRepository>();

		services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
		services.AddScoped<IPasswordHasher, PasswordHasher>();
		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IUserContextService, UserContextService>();

		return services;
	}
}
