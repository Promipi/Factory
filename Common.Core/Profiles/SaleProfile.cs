using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Profiles
{
    public sealed class SaleProfile : Profile
    {
        public SaleProfile()
        {

            CreateMap<Contracts.Sales.SaleCreateDto, Domain.Sale>()
                        .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.Select(x => new Domain.ProductSale
                        {
                            ProductId = x.ProductId,
                            Quantity = x.Quantity
                        }).ToList()))
                        .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<Contracts.Sales.SaleUpdateDto, Domain.Sale>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.Select(x => new Domain.ProductSale
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));
        }
    }
}
