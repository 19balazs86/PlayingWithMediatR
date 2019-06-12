using System.Collections.Generic;
using System.Linq;
using PlayingWithMediatR.Entities;

namespace PlayingWithMediatR.Infrastructure
{
  public static class DataBaseInitializer
  {
    public static void Initialize(this DataBaseContext context)
    {
      context.Database.EnsureCreated();

      if (context.Products.Any()) return;

      IEnumerable<Product> products = Enumerable.Range(1, 100).Select(x
        => new Product { Name = $"Product-{x}", Price = x * 10, Description = $"P{x}-Description" });

      context.Products.AddRange(products);

      context.SaveChanges();
    }
  }
}
