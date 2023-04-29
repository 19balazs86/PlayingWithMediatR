using MediatR;
using Serilog;

namespace PlayingWithMediatR.MediatR.Pipeline;

// This is loaded automatically when you AddMediatR.
public sealed class RequestPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Log.Debug($"RequestPipelineBehavior -> RequestType: {typeof(TRequest).Name}, ResponseType: {typeof(TResponse).Name}.");

        return next();
    }
}
