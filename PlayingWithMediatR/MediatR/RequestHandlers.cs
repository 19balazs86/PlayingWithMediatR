using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public async Task<IEnumerable<Product>> Handle(GetAllProduct request, CancellationToken cancelToken)
    {
      return await _dbContext.ActiveProducts().ToListAsync(cancelToken);
    }

    /// <summary>
    /// Handle: GetProductById
    /// </summary>
    public async Task<Product> Handle(GetProductById request, CancellationToken cancelToken)
    {
      return await _dbContext.ActiveProducts().FirstOrDefaultAsync(p => p.Id == request.Id, cancelToken);
    }

    /// <summary>
    /// Handle: CreateProduct
    /// </summary>
    public async Task<Product> Handle(CreateProduct request, CancellationToken cancelToken)
    {
      // Here you can use AutoMapper to mapping between CreateProduct and Product 
      Product product = new Product
      {
        Name        = request.Name,
        Price       = request.Price,
        Description = request.Description,
      };

      EntityEntry<Product> entry = await _dbContext.Products.AddAsync(product, cancelToken);

      await _dbContext.SaveChangesAsync(cancelToken);

      return entry.Entity;
    }
  }
}
