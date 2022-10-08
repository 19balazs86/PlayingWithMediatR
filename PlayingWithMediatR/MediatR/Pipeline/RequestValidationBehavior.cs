using FluentValidation;
using FluentValidation.Results;
using MediatR;
using PlayingWithMediatR.Exceptions;

namespace PlayingWithMediatR.MediatR.Pipeline
{
  public class RequestValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
  {
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
      _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
      if (!validateRequest(request, out var errors))
        throw new SummarizeValidationException(errors);

      return next();
    }

    private bool validateRequest(TRequest request, out Dictionary<string, string[]> errors)
    {
      errors = null;

      var validationContext = new ValidationContext<TRequest>(request);

      ValidationFailure[] failures = _validators
        .Select(v => v.Validate(validationContext))
        .SelectMany(res => res.Errors)
        .Where(vf => vf != null)
        .ToArray();

      if (failures.Length == 0)
        return true;

      errors = getErrors(failures);

      return false;
    }

    private static Dictionary<string, string[]> getErrors(ValidationFailure[] failures)
    {
      var errors = new Dictionary<string, string[]>();

      foreach (string propName in failures.Select(e => e.PropertyName).Distinct())
      {
        string[] propertyFailures = failures
          .Where(vf  => vf.PropertyName == propName)
          .Select(vf => vf.ErrorMessage)
          .ToArray();

        errors.Add(propName, propertyFailures);
      }

      return errors;
    }
  }
}
