using System;
using System.Collections.Generic;

namespace PlayingWithMediatR.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
  public class SummarizeValidationException : Exception
#pragma warning restore RCS1194 // Implement exception constructors.
  {
    public const string ErrorMessage = "One or more validation errors occurred.";

    public Dictionary<string, string[]> Errors { get; }

    public SummarizeValidationException(Dictionary<string, string[]> errors)
      : base(ErrorMessage)
    {
      Errors = errors;
    }
  }
}
