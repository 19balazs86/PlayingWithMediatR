using System.Linq;
using PlayingWithMediatR.Entities;

namespace PlayingWithMediatR.Infrastructure
{
  public class DataBaseInitializer
  {
    public static void Initialize(DataBaseContext context)
    {
      context.Database.EnsureCreated();

      if (context.Products.Any()) return;

      context.Products.AddRange(
        new Product { Name = "P1Name", Price = 10, Description = "P1-Description" },
        new Product { Name = "P2Name", Price = 20, Description = "P2-Description" },
        new Product { Name = "P3Name", Price = 30, Description = "P3-Description" });

      context.SaveChanges();
    }
  }
}
