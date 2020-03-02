using System.Linq;
using System.Threading.Tasks;
using Bogus;
using PlayingWithMediatR.Entities;

namespace PlayingWithMediatR.Infrastructure
{
  public static class DataBaseInitializer
  {
    public static async Task SeedAsync(this DataBaseContext context)
    {
      await context.Database.EnsureCreatedAsync();

      if (context.Products.Any()) return;

      var products = new Faker<Product>()
        //.RuleFor(p => p.Id, _ => id++) // No need. The auto increment works well.
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Number(10, 500))
        .RuleFor(p => p.Description, f => f.Lorem.Sentence())
        .RuleFor(p => p.CreatedDate, f => f.Date.Recent(10).ToUniversalTime())
        .RuleFor(p => p.CategoryEnum, f => f.PickRandom<CategoryEnum>())
        .Generate(100);

      await context.Products.AddRangeAsync(products);

      await context.SaveChangesAsync();
    }
  }
}
