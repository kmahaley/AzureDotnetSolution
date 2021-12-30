using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDbApplication.Dtos;
using MongoDbApplication.Models;
using MongoDbApplication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository repository;

        private readonly ILogger<ItemController> logger;

        private readonly IMapper mapper;

        public ItemController(
            IRepository repository,
            ILogger<ItemController> logger,
            IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateAsync(ItemDto itemDto)
        {
            var inputItem = mapper.Map<Item>(itemDto);
            var savedItem = await repository.CreateItemAsync(inputItem);
            var outputItemDto = mapper.Map<ItemDto>(savedItem);
            return Ok(outputItemDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> UpdateAsync(Guid id, ItemDto itemDto)
        {
            if(id != itemDto.Id)
            {
                return BadRequest();
            }
            var inputItem = mapper.Map<Item>(itemDto);
            var updatedItem = await repository.UpdateItemAsync(id, inputItem);
            var outputItemDto = mapper.Map<ItemDto>(updatedItem);
            return Ok(outputItemDto);
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
            var savedItem = await repository.GetItemAsync(id);
            if(savedItem == null)
            {
                return NotFound();
            }
            var outputItemDto = mapper.Map<ItemDto>(savedItem);
            return Ok(outputItemDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItemsAsync()
        {
            var items = await repository.GetItemsAsync();
            var outputItemDtos = items
                .Select(item => mapper.Map<ItemDto>(item));
            return Ok(outputItemDtos);
        }
    }
}
