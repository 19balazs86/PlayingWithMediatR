using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlayingWithMediatR.Exceptions;

public static class CustomErrorHandlerExtensions
{
    public const string ErrorMessage = "Internal Server Error from the ExceptionHandlingMiddleware.";

    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlerMiddleware>();

    public static void UseCustomErrors(this IApplicationBuilder appBuilder, IHostEnvironment environment)
        => appBuilder.Run(httpContext => writeResponse(httpContext, environment.IsDevelopment()));

    private static async Task writeResponse(HttpContext httpContext, bool includeDetails)
    {
        IExceptionHandlerFeature exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

        // No need to log the error because app.UseExceptionHandler provides an ExceptionHandlerMiddleware, and it handles the logging

        if (exceptionHandlerFeature is not null)
        {
            await ExceptionHandlerMiddleware.WriteResponseAsync(httpContext, exceptionHandlerFeature.Error, includeDetails);
        }
    }
}

// Middleware registration method you DON'T know
// Codewrinkles: https://youtu.be/UHdXZCgUKas
public sealed class ExceptionHandlerMiddleware
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly RequestDelegate _next;

    private readonly bool _includeDetails;

    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment environment, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next           = next;
        _includeDetails = environment.IsDevelopment();
        _logger         = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            if (ex is not SummarizeValidationException)
            {
                _logger.LogError(ex, CustomErrorHandlerExtensions.ErrorMessage);
            }

            await WriteResponseAsync(httpContext, ex, _includeDetails);
        }
    }

    public static async Task WriteResponseAsync(HttpContext httpContext, Exception ex, bool includeDetails)
    {
        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        var errorResponse = ex is SummarizeValidationException svException ?
            new ErrorResponse(svException.Errors, traceId) : // Handle as Validation Exception
            new ErrorResponse(ex, includeDetails, traceId);

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode  = errorResponse.Status.GetValueOrDefault();

        await JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, _jsonSerializerOptions);
    }
}
