using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.Infrastructure;

namespace PlayingWithMediatR.MediatR
{
  public class RequestHandlers :
    IRequestHandler<GetAllProduct, IEnumerable<ProductDto>>,
    IRequestHandler<GetProductById, ProductDto>,
    IRequestHandler<CreateProduct, ProductDto>
  {
    private readonly DataBaseContext _dbContext;
    private readonly IMapper _mapper;

    public RequestHandlers(DataBaseContext dbContext, IMapper mapper)
    {
      _dbContext = dbContext;
      _mapper    = mapper;
    }
    
    /// <summary>
    /// Handle: GetAllProduct
    /// </summary>
    public async Task<IEnumerable<ProductDto>> Handle(GetAllProduct request, CancellationToken ct)
    {
      return await _mapper.ProjectTo<ProductDto>(_dbContext.ActiveProducts).ToListAsync(ct);
    }

    /// <summary>
    /// Handle: GetProductById
    /// </summary>
    public async Task<ProductDto> Handle(GetProductById request, CancellationToken ct)
    {
      return await _mapper.ProjectTo<ProductDto>(_dbContext.ActiveProducts).FirstOrDefaultAsync(p => p.Id == request.Id, ct);
    }

    /// <summary>
    /// Handle: CreateProduct
    /// </summary>
    public async Task<ProductDto> Handle(CreateProduct request, CancellationToken ct)
    {
      Product product = _mapper.Map<Product>(request);

      EntityEntry<Product> entry = await _dbContext.Products.AddAsync(product, ct);

      await _dbContext.SaveChangesAsync(ct);

      return _mapper.Map<ProductDto>(entry.Entity);
    }
  }
}
