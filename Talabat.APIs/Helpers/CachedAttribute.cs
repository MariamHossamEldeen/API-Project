using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;


        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cachedKey = GenerateCachedKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cachedService.GetCahchedResponseAsync(cachedKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                return;
            }

            var excutedEndPointContext = await next.Invoke();
            if(excutedEndPointContext.Result is OkObjectResult okObjectResult)
            {
                await cachedService.CacheResponseAsync(cachedKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCachedKeyFromRequest(HttpRequest request)
        {
            // {{baseUrl}}/api/Products?pageSize=6&pageIndex=1&order=name

            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path); // => /api/Products
            // pageSize=6
            // pageIndex=1
            // order=name 


            // => /api/Products
            foreach (var (key , value ) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
                // => /api/Products|pageSize-6
                // => /api/Products|pageSize-6|pageIndex-1
                // => /api/Products|pageSize-6|pageIndex-1|order-name

            }

            return keyBuilder.ToString();

        }
    }
}
