using DistributedCacheApplication.Models;
using DistributedCacheApplication.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace DistributedCacheApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : Controller
    {
        private readonly ILogger<RedisController> logger;

        private readonly IConnectionMultiplexer redis;

        private readonly IProductService productService;


        public RedisController(
            ILogger<RedisController> logger,
            IConnectionMultiplexer redis,
            IProductService productService)
        {
            this.logger = logger;
            this.redis = redis;
            this.productService = productService;
        }

        [HttpGet("{id}", Name = "GetValueByIdAsync")]
        public async Task<IActionResult> GetValueByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            string key = $"member-{id}";
            var redisDatabase = redis.GetDatabase();
            var cachedMember = redisDatabase.StringGet(key);

            if (string.IsNullOrWhiteSpace(cachedMember))
            {
                var nonCachedMember = await productService.GetProductAsync(id, cancellationToken);
                if (nonCachedMember is null)
                {
                    return NotFound();
                }

                var value = JsonConvert.SerializeObject(nonCachedMember);
                redisDatabase.StringSet(key, value);

                return Ok(nonCachedMember);
            }

            var cachedProduct = JsonConvert.DeserializeObject<Product>(cachedMember);
            return Ok(cachedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> SetValueAsync([FromBody] Product product, CancellationToken cancellationToken = default)
        {
            var addedMember = await productService.AddProductAsync(product, cancellationToken);
            
            string key = $"member-{product.ProductId}";
            string value = JsonConvert.SerializeObject(addedMember);
            var redisDatabase = redis.GetDatabase();
            redisDatabase.StringSet(key, value);
            
            return CreatedAtRoute(nameof(GetValueByIdAsync), new { Id = product.ProductId} , addedMember);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValueAsync(int id, CancellationToken cancellationToken = default)
        {
            string key = $"member-{id}";
            var redisDatabase = redis.GetDatabase();
            redisDatabase.StringGetDelete(key);
            await productService.DeleteProductAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpGet("allKeys")]
        public async Task<IActionResult> GetAllCacheKeysAsync(CancellationToken cancellationToken = default)
        {
            var redisKeys = redis.GetServer("localhost", 6379).Keys();
            var list = redisKeys.Select(key => (string)key).ToList();

            var redisValues = redisKeys
                .Select(key => (string)key)
                .ToList();

            List<string> listOfData = new List<string>();
            listOfData.AddRange(redisValues);

            return Ok(listOfData);
        }

        [HttpGet("allValues")]
        public async Task<IActionResult> GetAllCacheValuesAsync(CancellationToken cancellationToken = default)
        {
            var listOfData = new List<string>();
            var listOfProducts = new List<Product>();

            var redisKeys = redis.GetServer("localhost", 6379).Keys();

            var redisDatabase = redis.GetDatabase();
            var redisValues = redisKeys
                .Select(key => redisDatabase.StringGet(key))
                //.Select(value => value.ToString())
                .Select(value => JsonConvert.DeserializeObject<Product>(value))
                .ToList();

            //listOfData.AddRange(redisValues);
            listOfProducts.AddRange(redisValues);

            return Ok(listOfProducts);
        }
    }
}
