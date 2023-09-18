using Microsoft.EntityFrameworkCore;
using multiTenancy.Context;
using multiTenancy.Models;

namespace multiTenancy.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        public ProductService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Product> AddProduct(Product product)
        {
             context.Products.Add(product); 
             await context.SaveChangesAsync();  

            return product; 

        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
           return await context.Products.FindAsync(productId);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await context.Products.ToListAsync(); // context.products return iquerable so i must convert it to list because the return type of this function is list
                                                         // it apply the global filter qurey on this method 
        }
    }
}
