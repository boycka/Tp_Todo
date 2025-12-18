using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using Tp_TODO.ViewModels;


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

            // var json = HttpContext.Session.GetString("Todos");
            // List<TodoVm> todos;

            // if (string.IsNullOrEmpty(json))
            //{
            //  todos = new List<TodoVm>();
            //}
            //else
            //{
            //   todos = JsonSerializer.Deserialize<List<TodoVm>>(json) ?? new List<TodoVm>();
            //}
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
    }
}
