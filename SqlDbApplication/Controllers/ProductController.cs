using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Exceptions;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql;
using SqlDbApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SqlDbApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        private readonly ILogger<ProductController> logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
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
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            try
            {
                var existingEntity = await productService.GetProductByIdAsync(id);
                return Ok(existingEntity);
            }
            catch(ArgumentException ex)
            {
                return BadRequest();
            }
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync([FromBody] Product product)
        {
            logger.LogInformation("Adding data.---");
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
            var deletedProduct = await productService.DeleteProductByIdAsync(id);
            return Ok(deletedProduct);
        }

        /// <summary>
        /// This method will throw exception: Purposefully done
        /// This API demonstrates, API is adding data and updating in a FireAndForget manner on a new thread.
        /// - DbContext is scoped instance hence primary thread adds data.
        /// - new thread does not have DbContext scope instance hence update fails.
        /// </summary>
        [HttpPost("disposecontext")]
        public async Task<ActionResult<Product>> PostDisposeContextIssueAsync([FromBody] Product product)
        {
            logger.LogInformation("DisposeContextIssue: Adding data.---");
            var savedProduct = await productService.DisposeContextIssueAsync(product);
            return Ok(savedProduct);
        }

        /// <summary>
        /// This API demonstrates, API is adding data and updating in a FireAndForget manner on a new thread.
        /// - DbContext is scoped instance hence primary thread adds data.
        /// - new thread does not have DbContext scope instance hence update fails.
        /// </summary>
        [HttpPost("solvedisposecontext")]
        public async Task<ActionResult<Product>> PostSolveDisposeContextIssueAsync([FromBody] Product product)
        {
            logger.LogInformation("SolveDisposeContextIssue: Adding data.---");
            var savedProduct = await productService.SolveDisposeContextIssueAsync(product);
            return Ok(savedProduct);
        }
    }
}
