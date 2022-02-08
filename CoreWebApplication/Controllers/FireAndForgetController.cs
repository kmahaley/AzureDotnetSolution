using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CoreWebApplication.Models;
using CoreWebApplication.Repositories;
using CoreWebApplication.Services;
using Microsoft.Extensions.Logging;

namespace CoreWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FireAndForgetController : Controller
    {
        private readonly IBookService bookService;

        private readonly IRepository repository;

        private readonly ILogger<FireAndForgetController> logger;

        public FireAndForgetController(IBookService bookService, IRepository repository, ILogger<FireAndForgetController> logger)
        {
            this.bookService = bookService;
            this.repository = repository;
            this.logger = logger;
        }

        [HttpPut("quick/{id}")]
        public async Task<ActionResult<Item>> QuickUpdateAsync(Guid id, Item item)
        {
            logger.LogInformation($"{nameof(FireAndForgetController)} start processing");
            if(id != item.Id)
            {
                return BadRequest();
            }
            //_ = bookService.ProcessTaskAsync();
            return Ok();
        }
    }
}
