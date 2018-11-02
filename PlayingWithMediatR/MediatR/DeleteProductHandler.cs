﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

    /// <summary>
    /// Handle: DeleteProduct. This method does not have any return parameter.
    /// </summary>
    protected override async Task Handle(DeleteProduct request, CancellationToken cancelToken)
    {
      if (_random.Next(5) == 3)
        throw new DeleteProductException($"Something went wrong during deleting the product({request.Id})");

      Product product = new Product { Id = request.Id, IsDeleted = true };

      _dbContext.Entry(product).State = EntityState.Modified;

      await _dbContext.SaveChangesAsync(cancelToken);
    }
  }
}
