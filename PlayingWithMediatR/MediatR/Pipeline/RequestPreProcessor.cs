using MediatR.Pipeline;
using Serilog;

namespace PlayingWithMediatR.MediatR.Pipeline;

// This is loaded automatically when you AddMediatR.
public sealed class RequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
{
    public Task Process(TRequest request, CancellationToken ct)
    {
        Log.Debug($"RequestPreProcessor: {typeof(TRequest).Name}.");

        return Task.CompletedTask;
    }
}
