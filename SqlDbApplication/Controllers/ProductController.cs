using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly SqlRepositoryImpl _sqlRepositoryImpl;

        public ProductController(SqlRepositoryImpl sqlRepositoryImpl)
        {
            this._sqlRepositoryImpl = sqlRepositoryImpl ?? throw new ArgumentNullException(nameof(sqlRepositoryImpl));
        }


        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
        {
            return await _sqlRepositoryImpl.Products.ToListAsync();
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            var existingEntity = await _sqlRepositoryImpl.Products.FindAsync(id);
            if(existingEntity == null)
            {
                return BadRequest();
            }
            return Ok(existingEntity);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync([FromBody] Product product)
        {
            var savedProduct = await _sqlRepositoryImpl.AddAsync(product);
            _ = _sqlRepositoryImpl.SaveChangesAsync();
            return savedProduct.Entity;
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutAsync(int id, [FromBody] Product product)
        {
            var existingEntity = await _sqlRepositoryImpl.Products.FindAsync(id);
            if(existingEntity == null)
            { 
                return BadRequest();
            }
            existingEntity.Name = product.Name;
            existingEntity.Color = product.Color;
            existingEntity.UnitPrice = product.UnitPrice;
            existingEntity.AvailableQuantity = product.AvailableQuantity;
            _ = _sqlRepositoryImpl.SaveChangesAsync();
            return Ok(product);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteAsync(int id)
        {
            var existingEntity = await _sqlRepositoryImpl.Products.FindAsync(id);
            if(existingEntity == null)
            { 
                return BadRequest();
            }
            _sqlRepositoryImpl.Products.Remove(existingEntity);
            _ = _sqlRepositoryImpl.SaveChangesAsync();
            return Ok(existingEntity);
        }
    }
}
