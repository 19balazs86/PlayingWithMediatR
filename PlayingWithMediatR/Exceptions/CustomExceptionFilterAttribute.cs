using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace PlayingWithMediatR.Exceptions
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      if (context.Exception is SummarizeValidationException svException)
      {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        context.Result = new JsonResult(svException.Failures);
      }
      else
      {
        const string errorMessage = "I have found an error.";

        // To avoid multiple log.
        if (context.Exception is CustomExceptionBase customEx) customEx.LogErrorIfSo(errorMessage);
        else Log.Error(context.Exception, errorMessage);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.Result = new JsonResult(new { context.Exception.Message });
      }
    }
  }
}
