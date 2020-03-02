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
        .ForMember(p => p.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
        .ForMember(p => p.CategoryEnum, opt => opt.MapFrom(_ => CategoryEnum.Category1));

      CreateMap<Product, ProductDto>();
    }
  }
}
