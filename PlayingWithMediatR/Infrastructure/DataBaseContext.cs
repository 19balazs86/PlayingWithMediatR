﻿using System;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Reading the date from database it will set the CreatedDate as UTC kind.
      modelBuilder.Entity<Product>()
        .Property(p => p.CreatedDate)
        .HasConversion(to => to, from => DateTime.SpecifyKind(from, DateTimeKind.Utc));

      // This will save as a string in the database.
      modelBuilder.Entity<Product>()
       .Property(p => p.CategoryEnum)
       .HasConversion<string>();

      base.OnModelCreating(modelBuilder);
    }
  }
}
