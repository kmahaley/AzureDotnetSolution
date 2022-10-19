using DistributedCacheApplication.Filters;
using DistributedCacheApplication.Models;
using DistributedCacheApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistributedCacheApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ControllerFilter()]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        private readonly ILogger<ProductController> logger;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
        {
            var listOfProducts = await productService.GetAllProductsAsync();
            return Ok(listOfProducts);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        [HttpETagFilter]
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            logger.LogInformation("Adding data. GetAsync---");
            try
            {
                var existingEntity = await productService.GetProductAsync(id);
                return Ok(existingEntity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync([FromBody] Product product)
        {
            logger.LogInformation("Adding data. PostAsync---");
            var savedProduct = await productService.AddProductAsync(product);
            return Ok(savedProduct);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutAsync(int id, [FromBody] Product product)
        {
            var updatedProduct = await productService.UpdateProductAsync(id, product);
            return Ok(updatedProduct);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteAsync(int id)
        {
            await productService.DeleteProductAsync(id);
            return Ok(id);
        }

    }
}
