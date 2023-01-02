using DistributedCacheApplication.Models;
using DistributedCacheApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace DistributedCacheApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InMemoryCacheController : Controller
    {
        private readonly MemoryCacheEntryOptions Options = 
            new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMilliseconds(10));


        private readonly ILogger<InMemoryCacheController> logger;

        private readonly IMemoryCache memoryCache;

        private readonly IProductService productService;


        public InMemoryCacheController(
            ILogger<InMemoryCacheController> logger,
            IMemoryCache memoryCache,
            IProductService productService)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCacheValueAsync(int id)
        {
            Product savedProduct;
            var ispresent = memoryCache.TryGetValue(id, out savedProduct);
            if (ispresent)
            {
                logger.LogInformation("returing from in memory cache");
                return Ok(savedProduct);
                
            }
            var repoSavedProduct = await productService.GetProductAsync(id);
            if (repoSavedProduct == null)
            {
                return NotFound();
            }
            return Ok(repoSavedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> AddCacheValueAsync([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product request data failed validation");
            }

            var _ = memoryCache.Set(product.ProductId, product);
            var savedProduct = await productService.AddProductAsync(product);

            return Ok(savedProduct);
        }

        [HttpPost("expiration")]
        public async Task<IActionResult> AddCacheValueWithExpirationAsync([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product request data failed validation");
            }

            var _ = memoryCache.Set(product.ProductId, product, Options);
            var savedproduct = await productService.AddProductAsync(product);

            return Ok(savedproduct);
        }

        [HttpPost("getOrCreate")]
        public async Task<IActionResult> GetorCreateCacheValueAsync([FromBody] Product product)
        {
            var savedProduct = await memoryCache.GetOrCreateAsync(
                product.ProductId,
                async entryKey => await productService.AddProductAsync(product));
            

            return Ok(savedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCacheValueAsync(int id)
        {
            memoryCache.Remove(id);
            await productService.DeleteProductAsync(id);

            return Ok(id);
        }
    }
}
