using FluentValidation;
using PlayingWithMediatR.MediatR;

namespace PlayingWithMediatR.Validation
{
  public class ProductValidator : AbstractValidator<CreateProduct>
  {
    public ProductValidator()
    {
      RuleFor(cp => cp.Name).NotNull().MinimumLength(5).MaximumLength(50);
      RuleFor(cp => cp.Price).GreaterThan(0);
      RuleFor(cp => cp.Description).NotNull().MinimumLength(5).MaximumLength(250);
    }
  }
}
