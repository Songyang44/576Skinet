using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entity;

namespace API.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            //调用 CreateMap 方法，并传递 Product 和 ProductToReturnDto 作为类型参数。
            //它创建了一个映射关系，将 Product 类型的对象映射到 ProductToReturnDto 类型的对象。
            //Call the CreateMap method and pass Product and ProductToReturnDto as type parameters. 
            //It creates a mapping relationship that maps objects of type Product to objects of type ProductToReturnDto.
            CreateMap<Product,ProductToReturnDto>()
            .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name))
            .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
            .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductUrlResolver>());
        }
    }
}