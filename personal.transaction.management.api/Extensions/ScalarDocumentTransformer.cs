using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace personal.transaction.management.api.Extensions;

internal sealed class ScalarDocumentTransformer : IOpenApiDocumentTransformer
{
	public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
	{
		document.Info.Title = "Personal Transaction Management API";
		document.Info.Version = "v1";
		document.Info.Description = "REST API for managing personal financial transactions.";

		document.Components ??= new OpenApiComponents();
		document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
		{
			["Bearer"] = new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				BearerFormat = "JWT"
			}
		};

		// Aplica el esquema a todos los endpoints
		foreach (var operation in document.Paths.Values.SelectMany(p => p.Operations))
		{
			operation.Value.Security ??= new List<OpenApiSecurityRequirement>();
			operation.Value.Security.Add(new OpenApiSecurityRequirement
			{
				[new OpenApiSecuritySchemeReference("Bearer", document)] = []
			});
		}

		return Task.CompletedTask;
	}
}