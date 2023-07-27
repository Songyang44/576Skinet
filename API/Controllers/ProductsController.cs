using API.Dtos;
using API.Error;
using API.Helpers;
using AutoMapper;
using Core.Entity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{



    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> productBrandRepo,
            IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper
        )
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }



        //get a list of products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery]ProductSecParams productParams){
            var spec=new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _productRepo.CountAsync(countSpec);
            var products=await _productRepo.ListAsync(spec);
            var data=_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,totalItems,data));
        }

        //get a product
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        /*
        ProducesResponseType 是 ASP.NET Core 中的一个特性（Attribute），
        用于标记控制器的操作方法（Action）所返回的数据类型和HTTP状态码。
        该特性可以用于为 API 文档生成工具提供元数据，
        以帮助客户端开发人员了解API的预期响应类型和状态码。
        */
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id){
            var spec= new ProductsWithTypesAndBrandsSpecification(id);
            var product=await _productRepo.GetEntityWithSpec(spec);
            return _mapper.Map<Product,ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }


    }
}