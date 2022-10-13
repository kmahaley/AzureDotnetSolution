﻿using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace DistributedCacheApplication.Filters
{
    public class ControllerFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly string filterName;

        public ControllerFilter(string filterName)
        {
            this.filterName = filterName;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            Debug.WriteLine($"before controller filter. {filterName}");


            var executedContext = await next();

            Debug.WriteLine($"after controller filter. {filterName}");

        }


    }
}