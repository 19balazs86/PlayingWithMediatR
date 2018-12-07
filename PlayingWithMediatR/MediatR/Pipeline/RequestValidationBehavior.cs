using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    public Task<TResponse> Handle(TRequest request, CancellationToken cancelToken, RequestHandlerDelegate<TResponse> next)
    {
      ValidationContext validationContext = new ValidationContext(request);

      ValidationFailure[] failures = _validators
        .Select(v => v.Validate(validationContext))
        .SelectMany(res => res.Errors)
        .Where(vf => vf != null)
        .ToArray();

      if (failures.Length != 0)
        throw new SummarizeValidationException(failures);

      return next();
    }
  }
}
