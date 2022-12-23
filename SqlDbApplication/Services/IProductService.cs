using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product> DeleteProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> UpdateProductAsync(int id, Product product);

        // This method will fail. Purposefully done.
        Task<Product> DisposeContextIssueAsync(Product product);

        // Dirty approach to use Fire and Forget mechanism
        Task<Product> SolvedDisposeContextIssueDirtyApproachAsync(Product product);

        // Correct approach to use Fire and Forget mechanism
        Task<Product> SolveDisposeContextIssueAsync(Product product);
    }
}