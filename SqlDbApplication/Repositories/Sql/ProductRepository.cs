using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlDatabaseContext databaseContext;

        private readonly ILogger<ProductRepository> logger;


        public ProductRepository(SqlDatabaseContext databaseContext, ILogger<ProductRepository> logger)
        {
            this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var list = await databaseContext.Products.ToListAsync();
            return list;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var existingEntity = await databaseContext.Products.FindAsync(id);
            if(existingEntity == null)
            {
                throw new ArgumentException($"Product with Id:{id} does not exists.");
            }
            return existingEntity;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var savedProduct = await databaseContext.AddAsync(product);
            _ = databaseContext.SaveChangesAsync();
            return savedProduct.Entity;
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await GetProductByIdAsync(id);
            existingProduct.Name = product.Name;
            existingProduct.Color = product.Color;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.AvailableQuantity = product.AvailableQuantity;
            _ = databaseContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProductByIdAsync(int id)
        {
            var existingProduct = await GetProductByIdAsync(id);
            databaseContext.Products.Remove(existingProduct);
            _ = databaseContext.SaveChangesAsync();
            return existingProduct;
        }
    }
}
