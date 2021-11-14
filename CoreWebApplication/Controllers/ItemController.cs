using CoreWebApplication.Dtos;
using CoreWebApplication.Dtos.Extensions;
using CoreWebApplication.Models;
using CoreWebApplication.Models.Extensions;
using CoreWebApplication.Repositories;
using Microsoft.AspNetCore.Http;
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

        public ItemController(IRepository repository, ILogger<ItemController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpPost]
        public ActionResult<ItemDto> Create(ItemDto itemDto)
        {
            var addedItem = repository.CreateItem(itemDto.AsItem());
            return Ok(addedItem.AsItemDto());
        }

        [HttpPut("{id}")]
        public ActionResult<ItemDto> Create(Guid id, ItemDto itemDto)
        {
            if(id != itemDto.Id)
            {
                return BadRequest();
            }
            var updatedItem = repository.UpdateItem(id, itemDto.AsItem());
            return Ok(updatedItem.AsItemDto());
        }

        [HttpDelete]
        public ActionResult Delete(Guid id)
        {
            return Ok(repository.DeleteItem(id));
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id);
            if(item == null)
            {
                return NotFound();
            }
            return item.AsItemDto();
        }

        [HttpGet]
        public ActionResult<IEnumerable<ItemDto>> GetItems()
        {
            var itemDtos = repository.GetItems().Select(item => item.AsItemDto());
            return Ok(itemDtos);
        }


    }
}
