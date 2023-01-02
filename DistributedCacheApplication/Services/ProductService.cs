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

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            logger.LogInformation("--- called ProductService: Cache missed. GetAllProductsAsync");
            return await productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            logger.LogInformation("--- called ProductService: Cache missed. GetProductAsync");
            return await productRepository.GetProductAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {

            logger.LogInformation("--- called ProductService: Adding product. AddProductAsync");
            return await productRepository.AddProductAsync(product);
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            logger.LogInformation("--- called ProductService: updating product. UpdateProductAsync");
            return await productRepository.UpdateProductAsync(id, product);
        }

        public async Task DeleteProductAsync(int id)
        {
            logger.LogInformation("--- called ProductService: delete product. DeleteProductAsync");
            await productRepository.DeleteProductAsync(id);
        }
    }
}
