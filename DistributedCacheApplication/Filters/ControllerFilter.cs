using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace DistributedCacheApplication.Filters
{
    public class ControllerFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly string filterName = nameof(ControllerFilter);

        private readonly ILogger<ControllerFilter> logger;

        public ControllerFilter(ILogger<ControllerFilter> logger)
        {
            this.logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            logger.LogInformation($"--- before controller filter. {filterName}");


            var executedContext = await next();

            logger.LogInformation($"--- after controller filter. {filterName}");

        }


    }
}