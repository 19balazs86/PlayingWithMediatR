using System;

namespace PlayingWithMediatR.Entities
{
  public class ProductDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CategoryEnum { get; set; }
  }
}
