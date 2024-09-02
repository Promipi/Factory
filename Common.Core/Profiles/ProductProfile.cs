using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Profiles
{
    public sealed class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Contracts.Products.ProductCreateDto,Domain.Product>();
            CreateMap<Contracts.Products.ProductUpdateDto,Domain.Product>();
        }
    }
}
