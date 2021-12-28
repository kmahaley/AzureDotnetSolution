using CoreWebApplication.Dtos;
using CoreWebApplication.Dtos.Extensions;
using CoreWebApplication.Models.Extensions;
using CoreWebApplication.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository repository;

        private readonly ILogger<ItemController> logger;

        public ItemController(IEnumerable<IRepository> repositories, ILogger<ItemController> logger)
        {
            this.repository = repositories.FirstOrDefault(repo => string.Equals(repo.GetRepositoryName, nameof(MongoDbRepository)));
            this.logger = logger;
        }

        //public ItemController(IRepository repository, ILogger<ItemController> logger)
        //{
        //    this.repository = repository;
        //    this.logger = logger;
        //}

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateAsync(ItemDto itemDto)
        {
            var addedItem = await repository.CreateItemAsync(itemDto.AsItem());
            return Ok(addedItem.AsItemDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> UpdateAsync(Guid id, ItemDto itemDto)
        {
            if(id != itemDto.Id)
            {
                return BadRequest();
            }
            var updatedItem = await repository.UpdateItemAsync(id, itemDto.AsItem());
            return Ok(updatedItem.AsItemDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var guid = await repository.DeleteItemAsync(id);
            return Ok(guid);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            return item.AsItemDto();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItemsAsync()
        {
            var items = await repository.GetItemsAsync();
            var itemDtos = items.Select(item => item.AsItemDto());
            return Ok(itemDtos);
        }


    }
}
