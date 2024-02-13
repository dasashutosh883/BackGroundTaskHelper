using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using BackgroundTaskHelper.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using BackgroundTaskHelper.CacheHelper;

namespace BackgroundTaskHelper.ActionFilters
{

    public class LogActionFilter : ActionFilterAttribute,IAsyncActionFilter
    {
        private readonly ICacheService _cacheService;
        public LogActionFilter(ICacheService cacheService)
        {
            _cacheService=cacheService;
        }
        public override async Task  OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            LogModel log = new LogModel()
            {
                Username = request.HttpContext.User.Identity!.Name ?? "Anonymous",
                IpAddress = request.HttpContext.Connection.RemoteIpAddress!.ToString(),
                AccessedURL = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request),
                Timestamp = DateTime.Now.ToString()
            };
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            _cacheService.AppendData<LogModel>("Audit",log, expirationTime);
            Console.WriteLine(context.RouteData.Values.Values.AsEnumerable().FirstOrDefault() + "-"+context.RouteData.Values.Values.AsEnumerable().LastOrDefault());
            await next();
        }

        public  async Task OnActionExecutedAsync(ActionExecutedContext context, ActionExecutionDelegate next)
        {
            await next();
        }
    }
}
