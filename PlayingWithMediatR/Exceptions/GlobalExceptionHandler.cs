using Microsoft.AspNetCore.Diagnostics;

namespace PlayingWithMediatR.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception handled by GlobalExceptionHandler");

        var hostEnvironment = httpContext.RequestServices.GetRequiredService<IHostEnvironment>();

        bool isDevelopment = hostEnvironment.IsDevelopment();

        await ExceptionHandlerMiddleware.WriteResponseAsync(httpContext, exception, isDevelopment);

        return true;
    }
}
