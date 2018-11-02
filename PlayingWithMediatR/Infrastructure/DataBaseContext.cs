using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Entities;

namespace PlayingWithMediatR.Infrastructure
{
  public class DataBaseContext : DbContext
  {
    public DbSet<Product> Products { get; set; }

    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {

    }
  }

  public static class ProductExtension
  {
    public static IQueryable<Product> ActiveProducts(this DataBaseContext dbContext)
      => dbContext.Products.Where(p => !p.IsDeleted);
  }
}
