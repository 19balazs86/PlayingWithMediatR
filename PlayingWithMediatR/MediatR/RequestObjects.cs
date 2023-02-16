using MediatR;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.Pagination;

namespace PlayingWithMediatR.MediatR;

/// <summary>
/// Request for get all product.
/// </summary>
public sealed class GetAllProduct : PageQuery, IRequest<PageResult<ProductDto>>
{
    public GetAllProduct() : base() { }
}

/// <summary>
/// Request for get a product by id.
/// </summary>
public sealed class GetProductById : IRequest<ProductDto>
{
    public int Id { get; set; }

    public GetProductById(int id)
    {
        Id = id;
    }
}

/// <summary>
/// Request for create a product.
/// </summary>
public sealed class CreateProduct : IRequest<ProductDto>
{
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
}

/// <summary>
/// Request for delete a product. This request is without return value.
/// </summary>
public sealed class DeleteProduct : IRequest
{
    public int Id { get; set; }

    public DeleteProduct(int id)
    {
        Id = id;
    }
}
