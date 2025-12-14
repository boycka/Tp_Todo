using Microsoft.AspNetCore.Mvc;
using Tp_TODO.ViewModels;
using System.Text.Json;


namespace Tp_TODO.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("isAuth") != "true")
            {
                return RedirectToAction("Login", "User");
            }

            var json = HttpContext.Session.GetString("Todos");
            List<TodoVm> todos;

            if (string.IsNullOrEmpty(json))
            {
                todos = new List<TodoVm>();
            }
            else
            {
                todos = JsonSerializer.Deserialize<List<TodoVm>>(json) ?? new List<TodoVm>();
            }

            return View(todos);
        }
    }
}
