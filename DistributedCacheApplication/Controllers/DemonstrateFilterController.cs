using DistributedCacheApplication.Attributes;
using DistributedCacheApplication.Filters;
using DistributedCacheApplication.Models;
using DistributedCacheApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistributedCacheApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ControllerFilter))]
    public class DemonstrateFilterController : ControllerBase
    {
        private readonly IProductService productService;

        private readonly ILogger<DemonstrateFilterController> logger;

        public DemonstrateFilterController(ILogger<DemonstrateFilterController> logger, IProductService productService)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        // GET: api/<ProductController>
        [HttpGet]
        [Throttling("GetAllAsync", "get all products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var listOfProducts = await productService.GetAllProductsAsync(cancellationToken);
            return Ok(listOfProducts);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        [Throttling("GetAsync", "get a product", RouteParameter = "id")]
        [ServiceFilter(typeof(HttpETagFilter))]
        public async Task<ActionResult<Product>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var attributeList = HttpContext.GetEndpoint().Metadata.GetOrderedMetadata<ThrottlingAttribute>();
            var itermList = HttpContext.Items.TryAdd("key", "after hitting API");
            logger.LogInformation("Action--------------------- > GetAsync API---");
            try
            {
                var existingEntity = await productService.GetProductAsync(id, cancellationToken);
                return Ok(existingEntity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync([FromBody] Product product, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Action--------------------- > PostAsync API---");
            var savedProduct = await productService.AddProductAsync(product, cancellationToken);
            return Ok(savedProduct);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutAsync(int id, [FromBody] Product product, CancellationToken cancellationToken = default)
        {
            var updatedProduct = await productService.UpdateProductAsync(id, product, cancellationToken);
            return Ok(updatedProduct);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await productService.DeleteProductAsync(id, cancellationToken);
            return Ok(id);
        }

    }
}
