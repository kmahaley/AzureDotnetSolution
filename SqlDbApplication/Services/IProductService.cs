using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product> DeleteProductByIdAsync(int id);
        Task<Product> DisposeContextIssueAsync(Product product);
        Task<Product> SolveDisposeContextIssueAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> UpdateProductAsync(int id, Product product);
    }
}