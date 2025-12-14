using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Tp_TODO.Filters;
using Tp_TODO.ViewModels;

namespace Tp_TODO.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]

    public class TodoController : Controller
    {
        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString("isAuth") == "true";
        }

        private List<TodoVm> GetTodos()
        {
            var json = HttpContext.Session.GetString("Todos");

            if (string.IsNullOrEmpty(json))
                return new List<TodoVm>();

            return JsonSerializer.Deserialize<List<TodoVm>>(json) ?? new List<TodoVm>();
        }

        private void SaveTodos(List<TodoVm> todos)
        {
            var json = JsonSerializer.Serialize(todos);
            HttpContext.Session.SetString("Todos", json);
        }


        public IActionResult Index()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "User");

            var todos = GetTodos();
            return View(todos); 
        }

        public IActionResult Creat()   
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "User");

            return View();    
        }

        [HttpPost]
        public IActionResult Creat(TodoVm vm)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "User");

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var todos = GetTodos();
            todos.Add(vm);
            SaveTodos(todos);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Show(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "User");

            var todos = GetTodos();

            if (id < 0 || id >= todos.Count)
            {
                return NotFound();
            }

            var vm = todos[id];
            return View(vm); 
        }

        public IActionResult Edit(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "User");

            var todos = GetTodos();

            if (id < 0 || id >= todos.Count)
            {
                return NotFound();
            }

            var vm = todos[id];
            ViewBag.Index = id;  
            return View(vm);    
        }

        [HttpPost]
        public IActionResult Edit(int id, TodoVm vm)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "User");

            if (!ModelState.IsValid)
            {
                ViewBag.Index = id;
                return View(vm);
            }

            var todos = GetTodos();

            if (id < 0 || id >= todos.Count)
            {
                return NotFound();
            }

            todos[id] = vm;
            SaveTodos(todos);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "User");

            var todos = GetTodos();

            if (id < 0 || id >= todos.Count)
            {
                return NotFound();
            }

            todos.RemoveAt(id);
            SaveTodos(todos);

            return RedirectToAction(nameof(Index));
        }
    }
}
