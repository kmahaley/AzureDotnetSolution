﻿using CoreWebApplication.Models;
using CoreWebApplication.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<ActionResult<Item>> CreateAsync(Item item)
        {
            var addedItem = await repository.CreateItemAsync(item);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var guid = await repository.DeleteItemAsync(id);
            return Ok(guid);
        }

        [HttpGet("{id}")]
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
