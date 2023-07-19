using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entity;

namespace API.Helpers
{

    //实现了 AutoMapper 的 IValueResolver 接口。它通过构造函数注入 IConfiguration 对象来获取应用程序的配置信息。
    //在 Resolve 方法中，它根据源对象的 PictureUrl 属性以及配置中的 API URL，返回完整的图片 URL。
    //如果源对象的 PictureUrl 为空或为 null，则返回 null。这个解析器可以用于在映射过程中处理产品的图片 URL，并将其转换为完整的可访问 URL。
    //Implemented the IValueResolver interface of AutoMapper. 
    //It injects the IConfiguration object through the constructor to obtain the configuration information of the application. 
    //In the Resolve method, it returns the complete image URL based on the PictureUrl property of the source object and the API URL in the configuration. 
    //If the PictureUrl of the source object is empty or null, it returns null. This parser can be used to process product image URLs during the mapping process and convert them into fully accessible URLs.
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;

        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"]+source.PictureUrl;
            }
            return null;
        }
    }
}