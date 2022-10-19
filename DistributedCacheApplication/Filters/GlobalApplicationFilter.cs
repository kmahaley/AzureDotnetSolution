using DistributedCacheApplication.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DistributedCacheApplication.Filters
{
    public class GlobalApplicationFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly string filterName = nameof(GlobalApplicationFilter);

        private readonly ILogger<GlobalApplicationFilter> logger;

        public GlobalApplicationFilter(ILogger<GlobalApplicationFilter> logger)
        {
            this.logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            logger.LogInformation($"--- before global filter. {filterName}");


            var executedContext = await next();

            logger.LogInformation($"--- after global filter. {filterName}");
            
        }

        
    }
}