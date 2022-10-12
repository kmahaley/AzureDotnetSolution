using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<Product> UpdateProductAsync(int id, Product product);
    }
}