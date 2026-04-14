using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.exceptions;
using System.Net;
using System.Text.Json;

namespace personal.transaction.management.api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var (statusCode, title, detail, errors) = MapException(exception);

		var problemDetails = new ProblemDetails
		{
			Status = statusCode,
			Title = title,
			Detail = detail,
			Instance = context.Request.Path
		};

		if (errors is not null)
			problemDetails.Extensions["errors"] = errors;

		context.Response.ContentType = "application/problem+json";
		context.Response.StatusCode = statusCode;

		var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		});

		return context.Response.WriteAsync(json);
	}

	private static (int StatusCode, string Title, string Detail, object? Errors) MapException(Exception exception)
	{
		return exception switch
		{
			ValidationException ex => (
				(int)HttpStatusCode.BadRequest,
				"Validation failed",
				"One or more validation errors occurred.",
				ex.Errors
					.GroupBy(e => e.PropertyName)
					.ToDictionary(
						g => g.Key,
						g => g.Select(e => e.ErrorMessage).ToArray())),

			NotFoundException ex => (
				(int)HttpStatusCode.NotFound,
				"Resource not found",
				ex.Message,
				null),

			ConflictException ex => (
				(int)HttpStatusCode.Conflict,
				"Conflict",
				ex.Message,
				null),

			UnauthorizedException ex => (
				(int)HttpStatusCode.Unauthorized,
				"Unauthorized",
				ex.Message,
				null),

			// 403 – forbidden domain rules
			SystemCategoryModificationException or
			SystemCategoryDeactivationException or
			SystemTagModificationException or
			UnauthorizedTagAccessException => (
				(int)HttpStatusCode.Forbidden,
				"Forbidden",
				exception.Message,
				null),

			// 422 – business rule violations
			InsufficientFundsException or
			FutureDateTransactionException or
			TransferIdRequiredException or
			TransferIdForbiddenException or
			TransferPartialModificationException => (
				(int)HttpStatusCode.UnprocessableEntity,
				"Business rule violation",
				exception.Message,
				null),

			DomainValidationException ex => (
				(int)HttpStatusCode.BadRequest,
				"Domain validation error",
				ex.Message,
				null),

			DomainException ex => (
				(int)HttpStatusCode.BadRequest,
				"Domain error",
				ex.Message,
				null),

			_ => (
				(int)HttpStatusCode.InternalServerError,
				"An unexpected error occurred",
				"An internal server error has occurred.",
				null)
		};
	}
}
