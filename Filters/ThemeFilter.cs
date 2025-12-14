using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tp_TODO.Filters
{
    public class ThemeFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var http = context.HttpContext;

            var theme = http.Request.Cookies.TryGetValue("theme", out var value) && !string.IsNullOrWhiteSpace(value)
                ? value
                : "light";

            if (context.Controller is Controller c)
                c.ViewData["Theme"] = theme;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
