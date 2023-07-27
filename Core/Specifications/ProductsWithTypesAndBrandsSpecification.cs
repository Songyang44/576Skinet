using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entity;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSecParams productParams)
        :base(x=>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))&& 
                (!productParams.BrandId.HasValue || x.ProductBrandId==productParams.BrandId)&&
                (!productParams.TypeId.HasValue || x.ProductTypeId==productParams.TypeId)
        )
        {
            //AddInclude(x => x.ProductType);：
            //这行代码调用 AddInclude 方法，并传递一个 lambda 表达式 x => x.ProductType 作为参数。
            //它将 ProductType 属性添加到规范的包含列表中，以表示需要包含产品类型。
            //Call the AddInclude method and pass a lambda expression x=>x.ProductType as a parameter. 
            //It adds the ProductType attribute to the inclusion list of the specification to indicate that the product type needs to be included.
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
            AddOrderBy(x=>x.Name);
            ApplyPaging(productParams.PageSize*(productParams.PageIndex-1),productParams.PageSize);
            if(!string.IsNullOrEmpty(productParams.sort))
            {
                switch(productParams.sort)
                {
                    case "priceAsc":
                    AddOrderBy(p=>p.Price);
                    break;

                    case "priceDesc":
                    AddOrderByDescending(p=>p.Price);
                    break;

                    default:
                    AddOrderBy(n=>n.Name);
                    break;

                }
            }
           


        }

        public ProductsWithTypesAndBrandsSpecification(int id):base(x=>x.Id==id)
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);

        }
    }
}