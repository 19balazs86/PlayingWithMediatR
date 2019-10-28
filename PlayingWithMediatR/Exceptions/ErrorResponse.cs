using System.Collections.Generic;
using System.Net;

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
      StatusCode       = (int)HttpStatusCode.BadRequest;
      Message          = SummarizeValidationException.ErrorMessage;
      ValidationErrors = validationErrors;
    }

    public ErrorResponse(string message)
    {
      StatusCode = (int)HttpStatusCode.InternalServerError;
      Message    = message;
    }
  }
}
