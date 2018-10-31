using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.Infrastructure;

namespace PlayingWithMediatR.MediatR
{
  public class RequestHandlers :
    IRequestHandler<GetAllProduct, IEnumerable<Product>>,
    IRequestHandler<GetProductById, Product>,
    IRequestHandler<CreateProduct, Product>
  {
    private readonly DataBaseContext _dbContext;

    public RequestHandlers(DataBaseContext dbContext)
    {
      _dbContext = dbContext;
    }

    /// <summary>
    /// Handle: GetAllProduct
    /// </summary>
    public Task<IEnumerable<Product>> Handle(GetAllProduct request, CancellationToken cancellationToken)
    {
      return Task.FromResult(_dbContext.Products.AsEnumerable());
    }

    /// <summary>
    /// Handle: GetProductById
    /// </summary>
    public Task<Product> Handle(GetProductById request, CancellationToken cancellationToken)
    {
      return Task.FromResult(_dbContext.Products.FirstOrDefault(p => p.Id == request.Id));
    }

    /// <summary>
    /// Handle: CreateProduct
    /// </summary>
    public async Task<Product> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
      int id = _dbContext.Products.Max(p => p.Id) + 1;

      Product product = new Product
      {
        Id          = id,
        Name        = request.Name,
        Price       = request.Price,
        Description = request.Description,
      };

      await _dbContext.Products.AddAsync(product);

      await _dbContext.SaveChangesAsync();

      return product;
    }
  }
}
