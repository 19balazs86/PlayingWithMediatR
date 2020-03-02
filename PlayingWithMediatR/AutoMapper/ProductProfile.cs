using System;
using AutoMapper;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.MediatR;

namespace PlayingWithMediatR.AutoMapper
{
  public class ProductProfile : Profile
  {
    public ProductProfile()
    {
      CreateMap<CreateProduct, Product>()
        .ForMember(p => p.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

      CreateMap<Product, ProductDto>();
    }
  }
}
