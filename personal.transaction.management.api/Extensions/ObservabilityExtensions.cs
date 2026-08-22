using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using personal.transaction.management.application.Common.Diagnostics;

namespace personal.transaction.management.api.Extensions;

public static class ObservabilityExtensions
{
    private const string ServiceName = "personal.transaction.management.api";

    public static IServiceCollection AddObservability(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var otlpEndpoint = configuration["OpenTelemetry:OtlpEndpoint"];

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ServiceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(ApplicationDiagnostics.ActivitySourceName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (string.IsNullOrWhiteSpace(otlpEndpoint))
                {
                    tracing.AddConsoleExporter();
                }
                else
                {
                    tracing.AddOtlpExporter(otlp => otlp.Endpoint = new Uri(otlpEndpoint));
                }
            });

        return services;
    }
}
