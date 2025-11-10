using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Models.Product;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            // Seed ProductBrands
            if (!dbContext.ProductBrands.Any()) // if it has no data
            {
                // Read text from Json file
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");

                // Serialize from Text to ProductBrand
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands is not null && brands.Count > 0)
                {
                    foreach (var brand in brands)
                        await dbContext.Set<ProductBrand>().AddAsync(brand);

                    await dbContext.SaveChangesAsync();
                }
            }


            // Seed ProductTypes
            if (!dbContext.ProductTypes.Any())
            {
                // Read text from Json file
                var typesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");

                // Serialize from Text to ProductType
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types is not null && types.Count > 0)
                {
                    foreach (var type in types)
                        await dbContext.Set<ProductType>().AddAsync(type);

                    await dbContext.SaveChangesAsync();
                }
            }


            // Seed Products
            if (!dbContext.Products.Any())
            {
                // Read text from Json file
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");

                // Serialize from Text to Product
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);


                if (products is not null && products.Count > 0)
                {
                    foreach (var product in products)
                        await dbContext.Set<Product>().AddAsync(product);

                    await dbContext.SaveChangesAsync();
                }
            }


            // Seed Delivery Methods
            if (!dbContext.DeliveryMethods.Any())
            {
                // Read text from Json file
                string deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");

                // Serialize from Text to DeliveryMethod
                List<DeliveryMethod>? deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods is not null && deliveryMethods.Count > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                        await dbContext.Set<DeliveryMethod>().AddAsync(deliveryMethod);

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
