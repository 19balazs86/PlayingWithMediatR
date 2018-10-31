using System.Collections.Generic;
using MediatR;
using PlayingWithMediatR.Entities;

namespace PlayingWithMediatR.MediatR
{
  /// <summary>
  /// Request for get all product.
  /// </summary>
  public class GetAllProduct : IRequest<IEnumerable<Product>>
  {
    // Empty
  }

  /// <summary>
  /// Request for get a product by id.
  /// </summary>
  public class GetProductById : IRequest<Product>
  {
    public int Id { get; set; }
  }

  /// <summary>
  /// Request for create a product.
  /// </summary>
  public class CreateProduct : IRequest<Product>
  {
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
  }

  /// <summary>
  /// Request for delete a product. This request is without return value.
  /// </summary>
  public class DeleteProduct : IRequest
  {
    public int Id { get; set; }
  }
}
