using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace PlayingWithMediatR.Exceptions
{
  public static class CustomErrorHandlerExtensions
  {
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
      => app.UseMiddleware<ExceptionHandlerMiddleware>();

    public static void UseCustomErrors(this IApplicationBuilder appBuilder, IHostEnvironment environment)
      => appBuilder.Run(httpContext => writeResponse(httpContext, environment.IsDevelopment()));

    private static async Task writeResponse(HttpContext httpContext, bool includeDetails)
    {
      IExceptionHandlerFeature exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

      if (exceptionHandlerFeature is object)
        await ExceptionHandlerMiddleware.WriteResponseAsync(httpContext, exceptionHandlerFeature.Error, includeDetails);
    }
  }

  public class ExceptionHandlerMiddleware
  {
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    private const string _errorMessage = "Internal Server Error from the ExceptionHandlingMiddleware.";

    private readonly RequestDelegate _next;

    private readonly bool _includeDetails;

    public ExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment environment)
    {
      _next           = next;
      _includeDetails = environment.IsDevelopment();
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        await WriteResponseAsync(httpContext, ex, _includeDetails);
      }
    }

    public static async Task WriteResponseAsync(HttpContext httpContext, Exception ex, bool includeDetails)
    {
      string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

      ErrorResponse errorResponse;

      if (ex is SummarizeValidationException svException) // Handle the Validation Exception.
        errorResponse = new ErrorResponse(svException.Errors, traceId);
      else
      {
        // To avoid multiple log.
        if (ex is CustomExceptionBase customEx)
          customEx.LogErrorIfSo(_errorMessage);
        else
          Log.Error(ex, _errorMessage);

        errorResponse = new ErrorResponse(ex, includeDetails, traceId);
      }

      httpContext.Response.ContentType = MediaTypeNames.Application.Json;
      httpContext.Response.StatusCode  = errorResponse.Status.GetValueOrDefault();

      await JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, _jsonSerializerOptions);
    }
  }
}
