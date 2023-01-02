using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Services
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken);
        Task DeleteProductAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
        Task<Product> GetProductAsync(int id, CancellationToken cancellationToken);
        Task<Product> UpdateProductAsync(int id, Product product, CancellationToken cancellationToken);
    }
}