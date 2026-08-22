using System.Diagnostics;
using MediatR;
using personal.transaction.management.application.Common.Diagnostics;

namespace personal.transaction.management.application.Common.Behaviors;

public sealed class TracingBehavior<TRequest, TResponse>(ActivitySource activitySource)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        using var activity = activitySource.StartActivity(
            $"MediatR {requestName}",
            ActivityKind.Internal);

        activity?.SetTag("mediatr.request_type", requestName);
        activity?.SetTag("mediatr.response_type", typeof(TResponse).Name);

        try
        {
            var response = await next(cancellationToken);
            activity?.SetStatus(ActivityStatusCode.Ok);
            return response;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.SetTag("exception.type", ex.GetType().FullName);
            activity?.AddException(ex);
            throw;
        }
    }
}
