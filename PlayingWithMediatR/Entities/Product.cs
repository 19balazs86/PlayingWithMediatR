using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayingWithMediatR.Entities
{
  public enum CategoryEnum
  {
    Category1 = 1, Category2, Category3
  }

  public class Product
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public CategoryEnum CategoryEnum { get; set; }
  }
}
