using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.Infrastructure;

namespace PlayingWithMediatR.MediatR
{
    /// <summary>
    /// RequestHandlers: There is another way to handle no return value request with IRequestHandler<DeleteProduct>.
    /// </summary>
    public class DeleteProductHandler : AsyncRequestHandler<DeleteProduct>
    {
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
            if (Random.Shared.NextDouble() < 0.2)
                throw new DeleteProductException($"Random error during deleting the product({request.Id})");

            var product = new Product { Id = request.Id, IsDeleted = true };

            EntityEntry<Product> entry = _dbContext.Attach(product);

            entry.Property(p => p.IsDeleted).IsModified = true;

            // Throw DbUpdateConcurrencyException, if the id is not exist.
            await _dbContext.SaveChangesAsync(cancelToken);
        }
    }
}
