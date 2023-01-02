using DistributedCacheApplication.Models;
using DistributedCacheApplication.Repository;
using System.Collections.Generic;

namespace DistributedCacheApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> logger;

        private readonly IProductRepository productRepository;

        public ProductService(ILogger<ProductService> logger, IProductRepository productRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("--- called ProductService: Cache missed. GetAllProductsAsync");
            return await productRepository.GetAllProductsAsync(cancellationToken);
        }

        public async Task<Product> GetProductAsync(int id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("--- called ProductService: Cache missed. GetProductAsync");
            return await productRepository.GetProductAsync(id, cancellationToken);
        }

        public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
        {

            logger.LogInformation("--- called ProductService: Adding product. AddProductAsync");
            return await productRepository.AddProductAsync(product, cancellationToken);
        }

        public async Task<Product> UpdateProductAsync(int id, Product product, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("--- called ProductService: updating product. UpdateProductAsync");
            return await productRepository.UpdateProductAsync(id, product, cancellationToken);
        }

        public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("--- called ProductService: delete product. DeleteProductAsync");
            await productRepository.DeleteProductAsync(id, cancellationToken);
        }
    }
}
