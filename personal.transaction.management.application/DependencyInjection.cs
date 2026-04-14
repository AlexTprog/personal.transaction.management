using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using personal.transaction.management.application.Common.Behaviors;
using System.Reflection;

namespace personal.transaction.management.application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}
