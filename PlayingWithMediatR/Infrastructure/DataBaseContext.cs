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
}
