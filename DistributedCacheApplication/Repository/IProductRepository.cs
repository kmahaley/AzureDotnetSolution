using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken);
        Task DeleteProductAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
        Task<Product> GetProductAsync(int id, CancellationToken cancellationToken);
        Task<Product> UpdateProductAsync(int id, Product product, CancellationToken cancellationToken);
    }
}