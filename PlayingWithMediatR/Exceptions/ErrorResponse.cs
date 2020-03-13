using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace PlayingWithMediatR.Exceptions
{
  public class ErrorResponse : ProblemDetails
  {
    private ErrorResponse(string traceId) : base()
    {
      if (!string.IsNullOrWhiteSpace(traceId))
        Extensions["traceId"] = traceId;
    }

    public ErrorResponse(Dictionary<string, string[]> validationErrors, string traceId)
      : this(traceId)
    {
      Status = Status400BadRequest; // using static
      Title  = SummarizeValidationException.ErrorMessage;

      Extensions["validationErrors"] = validationErrors;
    }

    public ErrorResponse(Exception ex, bool includeDetails, string traceId)
      : this(traceId)
    {
      Status = Status500InternalServerError;
      Title  = includeDetails ? $"An error occured: '{ex.Message}'" : "An error occured";
      Detail = includeDetails ? ex.ToString() : null;
    }
  }
}
