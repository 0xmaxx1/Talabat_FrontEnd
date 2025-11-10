using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.APIs.Helpers
{
    // built-in attribute
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            // {{BaseUrl}}/api/products?pageIndex=1&pageSize=5&sort=name

            StringBuilder keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path); // api/products


            // api/products
            foreach (var (key, value) in request.Query.OrderBy(k => k.Key)) // to order the query string data each request
            {
                keyBuilder.Append($"|{key}-{value}");
                // api/products|pageIndex-1
                // api/products|pageIndex-1|pageSize-5
                // api/products|pageIndex-1|pageSize-5|sort-name
            }

            return keyBuilder.ToString();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. Create Object from cache Service using Dependency Injection
            IResponseCacheService cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            // 2. Generate Cache Key
            string cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // 3. Get Cached Response from Cached Service by Cache Key
            string cachedResponse = await cachedService.GetCachedResponseAsync(cacheKey);

            // 4. Check on Cached Response if it has value(Cached before) and not equal Null
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                ContentResult contentResult = new ContentResult()
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }


            // 5. Check if Response is Null [Needed to execute the endpoint]
            ActionExecutedContext executedEndPointContext = await next.Invoke();

            // 6. Cache The Result from EndPoint
            if (executedEndPointContext.Result is OkObjectResult okObjectResult)
            {
                await cachedService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }
    }
}
