using Microsoft.AspNetCore.Diagnostics;

namespace PlayingWithMediatR.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly bool _isDevelopment;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService, IHostEnvironment hostEnvironment)
    {
        _problemDetailsService = problemDetailsService;

        _isDevelopment = hostEnvironment.IsDevelopment();
    }

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // There is no need to log the error, as app.UseExceptionHandler provides an ExceptionHandlerMiddleware that handles the logging
        // _logger.LogError(exception, "Exception handled by GlobalExceptionHandler");

        ProblemDetailsContext pdContext = exception.ToProblemDetailsContext(httpContext, includeDetails: _isDevelopment);

        httpContext.Response.StatusCode = pdContext.ProblemDetails.Status.GetValueOrDefault(StatusCodes.Status500InternalServerError);

        return _problemDetailsService.TryWriteAsync(pdContext);
    }
}
