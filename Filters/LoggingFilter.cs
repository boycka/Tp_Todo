using Microsoft.AspNetCore.Mvc.Filters;
using Tp_TODO.Services;

namespace Tp_TODO.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        private readonly IFileLog _writer;

        public LoggingFilter(IFileLog writer)
        {
            _writer = writer;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var http = context.HttpContext;

            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var login = http.User?.Identity?.IsAuthenticated == true
                ? (http.User.Identity?.Name ?? "Unknown")
                : "Anonymous";

            var controller = context.RouteData.Values["controller"]?.ToString() ?? "UnknownController";
            var action = context.RouteData.Values["action"]?.ToString() ?? "UnknownAction";

            await _writer.Write($"{date} – {login} – {controller} – {action}");

            await next();
        }
    }
}
