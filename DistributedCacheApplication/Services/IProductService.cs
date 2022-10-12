using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Services
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<Product> UpdateProductAsync(int id, Product product);
    }
}