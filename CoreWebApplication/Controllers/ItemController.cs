using CoreWebApplication.Models;
using CoreWebApplication.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository repository;

        private readonly ILogger<ItemController> logger;

        public ItemController(IRepository repository, ILogger<ItemController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(
            Description = "Create item",
            OperationId = "CreateAsync")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status201Created, "Item created", typeof(Item))]
        [SwaggerResponse(StatusCodes.Status200OK, "Item already exists in the system", typeof(Item))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect payload", typeof(Item))]
        public async Task<ActionResult<Item>> CreateAsync(Item item)
        {
            var addedItem = await repository.CreateItemAsync(item);
            return CreatedAtAction("RetrieveValue", new { id = item.Id }, item);
            
            return Ok(addedItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Item>> UpdateAsync(Guid id, Item item)
        {
            if(id != item.Id)
            {
                return BadRequest();
            }
            var updatedItem = await repository.UpdateItemAsync(id, item);
            return Ok(updatedItem);
        }

        [HttpPut("upsert/{id}")]
        public async Task<ActionResult<Item>> CreateOrUpdateItemAsync(Guid id, Item item)
        {
            logger.LogInformation($"{GetType().Name} started processing...");
            if(id != item.Id)
            {
                return BadRequest();
            }
            var added = await repository.CreateOrUpdateItemAsync(id, item);

            //_ = Task.Run(async () =>
            //{
            //    await repository.CreateOrUpdateItemAsync(id, item);
            //});

            return Ok(added);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var guid = await repository.DeleteItemAsync(id);
            return Ok(guid);
        }

        [HttpGet("{id}")]
        [ActionName("RetrieveValue")]
        public async Task<ActionResult<Item>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsAsync()
        {
            var items = await repository.GetItemsAsync();
            return Ok(items);
        }


    }
}
