﻿using FluentValidation;
using FluentValidation.Results;
using MediatR.Pipeline;
using PlayingWithMediatR.Exceptions;

namespace PlayingWithMediatR.MediatR.Pipeline;

public sealed class ValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPreProcessor(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        SummarizeValidationException validationException = await validateRequestAsync(request, cancellationToken);

        if (validationException is not null)
            throw validationException;

        //if (!validateRequest(request, out var errors))
        //    throw new SummarizeValidationException(errors);
    }

    private async Task<SummarizeValidationException> validateRequestAsync(TRequest request, CancellationToken ct)
    {
        foreach (IValidator<TRequest> validator in _validators)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
               return new SummarizeValidationException(validationResult.ToDictionary());
        }

        return null;
    }

    //private bool validateRequest(TRequest request, out Dictionary<string, string[]> errors)
    //{
    //    errors = null;

    //    var validationContext = new ValidationContext<TRequest>(request);

    //    ValidationFailure[] failures = _validators
    //      .Select(v => v.Validate(validationContext))
    //      .SelectMany(res => res.Errors)
    //      .Where(vf => vf != null)
    //      .ToArray();

    //    if (failures.Length == 0)
    //        return true;

    //    errors = getErrors(failures);

    //    return false;
    //}

    //private static Dictionary<string, string[]> getErrors(ValidationFailure[] failures)
    //{
    //    var errors = new Dictionary<string, string[]>();

    //    foreach (string propName in failures.Select(e => e.PropertyName).Distinct())
    //    {
    //        string[] propertyFailures = failures
    //          .Where(vf => vf.PropertyName == propName)
    //          .Select(vf => vf.ErrorMessage)
    //          .ToArray();

    //        errors.Add(propName, propertyFailures);
    //    }

    //    return errors;
    //}
}
