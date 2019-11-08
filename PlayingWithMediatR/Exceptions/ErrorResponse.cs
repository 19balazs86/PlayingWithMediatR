using System.Collections.Generic;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace PlayingWithMediatR.Exceptions
{
  public class ErrorResponse
  {
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string[]> ValidationErrors { get; set; }

    public ErrorResponse() { }

    public ErrorResponse(Dictionary<string, string[]> validationErrors)
    {
      StatusCode       = Status400BadRequest; // using static
      Message          = SummarizeValidationException.ErrorMessage;
      ValidationErrors = validationErrors;
    }

    public ErrorResponse(string message)
    {
      StatusCode = Status500InternalServerError;
      Message    = message;
    }
  }
}
