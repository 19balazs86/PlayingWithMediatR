using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace PlayingWithMediatR.Exceptions
{
  public static class ExceptionMiddlewareExtensions
  {
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
      return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
  }

  public class ExceptionHandlingMiddleware
  {
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { IgnoreNullValues = true };
    private const string _errorMessage = "Internal Server Error from the ExceptionHandlingMiddleware.";

    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        await handleExceptionAsync(httpContext, ex);
      }
    }

    private static async Task handleExceptionAsync(HttpContext httpContext, Exception exception)
    {
      ErrorResponse errorResponse;

      if (exception is SummarizeValidationException svException) // Handle the Validation Exception.
        errorResponse = new ErrorResponse(svException.Failures);
      else
      {
        // To avoid multiple log.
        if (exception is CustomExceptionBase customEx)
          customEx.LogErrorIfSo(_errorMessage);
        else
          Log.Error(exception, _errorMessage);

        // Here you can create a custom object / message.
        errorResponse = new ErrorResponse(exception.Message);
      }

      httpContext.Response.ContentType = MediaTypeNames.Application.Json;
      httpContext.Response.StatusCode  = errorResponse.StatusCode;

      await JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, _jsonSerializerOptions);
    }

    // This method can pass to the ApplicationBuilder.Run method in the Startup.
    public static async Task ApplicationBuilderRun(HttpContext context)
    {
      IExceptionHandlerFeature exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

      if (exceptionHandlerFeature != null)
        await handleExceptionAsync(context, exceptionHandlerFeature.Error);
    }
  }
}
