using Microsoft.AspNetCore.Mvc.Filters;

namespace Tp_TODO.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuth = context.HttpContext.Session.GetString("isAuth");
            if (isAuth != "true")
            {
                context.Result = new Microsoft.AspNetCore.Mvc.RedirectToActionResult("Login", "User", null);
            }
        }
    }
}
