using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
      int statusCode = (int)HttpStatusCode.InternalServerError;
      string responseText;

      if (exception is SummarizeValidationException svException)
      {
        // Handle the Validation Exception.
        statusCode   = (int)HttpStatusCode.BadRequest;
        responseText = JsonConvert.SerializeObject(new { StatusCode = statusCode, Error = svException.Failures });
      }
      else
      {
        const string errorMessage = "Internal Server Error from the ExceptionHandlingMiddleware.";

        // To avoid multiple log.
        if (exception is CustomExceptionBase customEx) customEx.LogErrorIfSo(errorMessage);
        else Log.Error(exception, errorMessage);

        // Here you can create a custom object / message.
        responseText = JsonConvert.SerializeObject(new { StatusCode = statusCode, Error = exception.Message });
      }

      httpContext.Response.ContentType = "application/json";
      httpContext.Response.StatusCode  = statusCode;

      await httpContext.Response.WriteAsync(responseText);
    }
  }
}
