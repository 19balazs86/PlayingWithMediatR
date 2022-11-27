using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace PlayingWithMediatR.Exceptions
{
    public class ErrorResponse : ProblemDetails
    {
        [JsonPropertyName("traceId")]
        public string TraceId { get; }

        [JsonPropertyName("errors")]
        public IDictionary<string, string[]> Errors { get; } // Like in the ValidationProblemDetails built-in class

        public ErrorResponse(string traceId)
          => TraceId = traceId;

        public ErrorResponse(IDictionary<string, string[]> validationErrors, string traceId) : this(traceId)
        {
            Status = Status400BadRequest; // using static
            Title  = SummarizeValidationException.ErrorMessage;

            Errors = validationErrors;
        }

        public ErrorResponse(Exception ex, bool includeDetails, string traceId) : this(traceId)
        {
            Status = Status500InternalServerError;
            Title  = includeDetails ? $"An error occured: '{ex.Message}'" : "An error occured";
            Detail = includeDetails ? ex.ToString() : null;
        }
    }
}
