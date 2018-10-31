using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.Infrastructure;

namespace PlayingWithMediatR.MediatR
{
  public class DeleteProductHandler : AsyncRequestHandler<DeleteProduct>
  {
    private readonly Random _random = new Random();
    private readonly DataBaseContext _dbContext;

    public DeleteProductHandler(DataBaseContext dbContext)
    {
      _dbContext = dbContext;
    }

    protected override async Task Handle(DeleteProduct request, CancellationToken cancellationToken)
    {
      if (_random.Next(5) == 3)
        throw new DeleteProductException($"Something went wrong during deleting the product({request.Id})");

      Product product = _dbContext.Products.FirstOrDefault(p => p.Id == request.Id);

      if (product == null) return;

      _dbContext.Products.Remove(product);

      await _dbContext.SaveChangesAsync();
    }
  }
}
