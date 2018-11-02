using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Entities;

namespace PlayingWithMediatR.Infrastructure
{
  public class DataBaseContext : DbContext
  {
    public DbSet<Product> Products { get; set; }

    public IQueryable<Product> ActiveProducts => Products.Where(p => !p.IsDeleted);

    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {

    }
  }
}
