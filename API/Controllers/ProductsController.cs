using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{



    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }



        //get a list of products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(){
            var products=await _context.Product.ToListAsync();
            return products;
        }

        //get a product
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            var product=await _context.Product.FindAsync(id);
            return product;
        }

    }
}