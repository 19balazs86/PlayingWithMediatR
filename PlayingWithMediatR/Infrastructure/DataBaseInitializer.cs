using System.Linq;
using Bogus;
using PlayingWithMediatR.Entities;

namespace PlayingWithMediatR.Infrastructure
{
  public static class DataBaseInitializer
  {
    public static void Initialize(this DataBaseContext context)
    {
      context.Database.EnsureCreated();

      if (context.Products.Any()) return;

      int id = 1;

      var products = new Faker<Product>()
        .RuleFor(p => p.Id, _ => id++)
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Number(10, 500))
        .RuleFor(p => p.Description, f => f.Lorem.Sentence())
        .Generate(100);

      context.Products.AddRange(products);

      context.SaveChanges();
    }
  }
}
