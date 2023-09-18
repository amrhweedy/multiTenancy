using multiTenancy.Models;

namespace multiTenancy.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync();

        Task<Product?> GetProductByIdAsync(int productId);

        Task<Product> AddProduct(Product product);
    }
}
