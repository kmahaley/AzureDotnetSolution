using Microsoft.AspNetCore.Mvc;
using SqlDbApplication.Exceptions;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<Product> GetAll()
        {
            return _sqlRepositoryImpl.Products.ToList();
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _sqlRepositoryImpl.Products.Find(id);
        }

        // POST api/<ProductController>
        [HttpPost]
        public Product Post([FromBody] Product product)
        {
            var savedProduct = _sqlRepositoryImpl.Add(product);
            _sqlRepositoryImpl.SaveChanges();
            return savedProduct.Entity;
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public Product Put(int id, [FromBody] Product product)
        {
            if(id != product.ProductId)
            {
                throw new Exception("Bad request!!!");
            }

            var savedProduct = _sqlRepositoryImpl.Add(product);
            _sqlRepositoryImpl.SaveChanges();
            return savedProduct.Entity;
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public Product Delete(int id)
        {
            var existingEntity = _sqlRepositoryImpl.Products.Find(id);
            if(existingEntity == null)
            { 
                throw new Exception("Bad request!!!");
            }
            _sqlRepositoryImpl.Products.Remove(existingEntity);
            _sqlRepositoryImpl.SaveChanges();
            return existingEntity;
        }
    }
}
