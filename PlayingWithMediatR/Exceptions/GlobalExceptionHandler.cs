using Microsoft.AspNetCore.Diagnostics;

namespace PlayingWithMediatR.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // No need to log the error because app.UseExceptionHandler provides an ExceptionHandlerMiddleware, and it handles the logging
        //_logger.LogError(exception, "Exception handled by GlobalExceptionHandler");

        var hostEnvironment = httpContext.RequestServices.GetRequiredService<IHostEnvironment>();

        bool isDevelopment = hostEnvironment.IsDevelopment();

        await ExceptionHandlerMiddleware.WriteResponseAsync(httpContext, exception, isDevelopment);

        return true;
    }
}
