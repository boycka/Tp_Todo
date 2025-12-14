using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // si tu n'as pas les global usings
using Tp_TODO.ViewModels;

namespace Tp_TODO.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string reversedPassword = new string(vm.Password.Reverse().ToArray());

            if (vm.Login == reversedPassword)
            {
                HttpContext.Session.SetString("isAuth", "true");
                HttpContext.Session.SetString("userEmail", vm.Login);

                return RedirectToAction("Index", "Todo");
            }

            ModelState.AddModelError(string.Empty, "Login ou mot de passe incorrect.");
            return View(vm);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
