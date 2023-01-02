using DistributedCacheApplication.Models;
using DistributedCacheApplication.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace DistributedCacheApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistributedCacheController : ControllerBase
    {
        private readonly ILogger<DistributedCacheController> logger;

        private readonly IDistributedCache distributedCache;

        private readonly IProductService productService;


        public DistributedCacheController(
            ILogger<DistributedCacheController> logger,
            IDistributedCache distributedCache,
            IProductService productService)
        {
            this.logger = logger;
            this.distributedCache = distributedCache;
            this.productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCacheValueAsync(int id, CancellationToken cancellationToken = default)
        {
            string key = $"member-{id}";
            var cachedMember = await distributedCache.GetStringAsync(key, cancellationToken);
            
            if (string.IsNullOrWhiteSpace(cachedMember))
            {
                var nonCachedMember = await productService.GetProductAsync(id, cancellationToken);
                if(nonCachedMember is null)
                {
                    return NotFound();
                }

                await distributedCache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(nonCachedMember),
                    cancellationToken);

                return Ok(nonCachedMember);
            }

            var cachedProduct = JsonConvert.DeserializeObject<Product>(cachedMember);
            return Ok(cachedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> SetCacheValueAsync([FromBody] Product product, CancellationToken cancellationToken = default)
        {
            var addedMember = await productService.AddProductAsync(product, cancellationToken);
            string key = $"member-{product.ProductId}";
            await distributedCache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(addedMember),
                cancellationToken);

            return Ok(addedMember);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCacheValueAsync(int id, CancellationToken cancellationToken = default)
        {
            string key = $"member-{id}";
            var task1 = productService.DeleteProductAsync(id, cancellationToken);
            var task2 = distributedCache.RemoveAsync(key, cancellationToken);

            await Task.WhenAll(task1, task2);

            return NoContent();
        }

        [HttpGet("allCacheData")]
        public async Task<IActionResult> GetCacheValueAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}