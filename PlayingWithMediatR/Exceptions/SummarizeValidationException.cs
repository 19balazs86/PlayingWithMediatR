using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace PlayingWithMediatR.Exceptions
{
  public class SummarizeValidationException : Exception
  {
    public const string ErrorMessage = "One or more validation errors occurred";

    public Dictionary<string, string[]> Failures { get; }

    public SummarizeValidationException() : base(ErrorMessage)
    {
      Failures = new Dictionary<string, string[]>();
    }

    public SummarizeValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
      IEnumerable<string> propNames = failures.Select(e => e.PropertyName).Distinct();

      foreach (string propName in propNames)
      {
        string[] propertyFailures = failures
          .Where(vf => vf.PropertyName == propName)
          .Select(vf => vf.ErrorMessage)
          .ToArray();

        Failures.Add(propName, propertyFailures);
      }
    }
  }
}
