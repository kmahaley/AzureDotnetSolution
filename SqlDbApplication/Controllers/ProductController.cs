using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Exceptions;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SqlDbApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        private readonly ILogger<ProductController> logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
        {
            var listOfProducts = await productRepository.GetAllProductsAsync();
            return Ok(listOfProducts);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            try
            {
                var existingEntity = await productRepository.GetProductByIdAsync(id);
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
            var savedProduct = await productRepository.AddProductAsync(product);
            return savedProduct;
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutAsync(int id, [FromBody] Product product)
        {
            var updatedProduct = await productRepository.UpdateProductAsync(id, product);
            return Ok(updatedProduct);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteAsync(int id)
        {
            var deletedProduct = await productRepository.DeleteProductByIdAsync(id);
            return Ok(deletedProduct);
        }
    }
}
