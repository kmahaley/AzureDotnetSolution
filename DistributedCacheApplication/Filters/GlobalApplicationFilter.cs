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

        public GlobalApplicationFilter()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            Debug.WriteLine($"before global filter. {filterName}");


            var executedContext = await next();

            Debug.WriteLine($"after global filter. {filterName}");
            
        }

        
    }
}