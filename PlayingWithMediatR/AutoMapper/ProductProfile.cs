using AutoMapper;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.MediatR;

namespace PlayingWithMediatR.AutoMapper
{
  public class ProductProfile : Profile
  {
    public ProductProfile()
    {
      CreateMap<CreateProduct, Product>();
      CreateMap<Product, ProductDto>();
    }
  }
}
