using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entity;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if(!context.ProductBrands.Any())
            {
                var brandsData=File.ReadAllText("../Infrastructure/SeedData/brands.json");
                var brands=JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
            }

            if(!context.ProductTypes.Any())
            {
                var typesData=File.ReadAllText("../Infrastructure/SeedData/types.json");
                var types=JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }

            if(!context.Product.Any())
            {
                var productsData=File.ReadAllText("../Infrastructure/SeedData/products.json");
                var products=JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Product.AddRange(products);
            }

            if(context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}