using MediatR;

namespace PlayingWithMediatR.MediatR.Pipeline;

public sealed class RequestPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public RequestPipelineBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogDebug("RequestPipelineBehavior -> RequestType: {RequestName}, ResponseType: {ResponseName}.",
            typeof(TRequest).Name, typeof(TResponse).Name);

        return next();
    }
}
