using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
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

            using (SqlConnection conn = new SqlConnection(
    "Data Source=.\\SQLEXPRESS;Initial Catalog=TodoDB;Integrated Security=True;Encrypt=false"))
            {
                List<TodoVm> todo = new List<TodoVm>();

                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT libelle, description, state, datelimit FROM dbo.Todo_Table",
                    conn
                );

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TodoVm vm = new TodoVm
                        {
                            Libelle = reader["libelle"].ToString(),
                            Description = reader["description"].ToString(),
                            State = Enum.Parse<Tp_TODO.Enums.State>(reader["state"].ToString()),
                            DateLimite = DateOnly.FromDateTime(reader.GetDateTime(3))
                        };

                        todo.Add(vm);
                    }
                }

                return View(todo);
            }
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

            using (SqlConnection conn = new SqlConnection(
    "Data Source=.\\SQLEXPRESS;Initial Catalog=TodoDb;Integrated Security=True;Encrypt=false"))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Todo_Table (libelle, description, state, datelimit) " +
                    "VALUES (@Libelle, @Description, @State, @DateLimite)", conn);

                cmd.Parameters.Add("@Libelle", SqlDbType.NVarChar).Value = vm.Libelle;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = vm.Description;
                cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = vm.State.ToString();
                cmd.Parameters.Add("@DateLimite", SqlDbType.Date)
                    .Value = vm.DateLimite.ToDateTime(TimeOnly.MinValue);

                cmd.ExecuteNonQuery();
            }


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
