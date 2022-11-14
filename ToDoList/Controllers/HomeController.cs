using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using MySql.Data.MySqlClient;
using ToDoList.Models.ViewModels;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
        }

        [HttpGet]

        public JsonResult PopulateForm(int Id)
        {
            var todo = GetById(Id);
            return Json(todo);
        }

        internal TodoViewModel GetAllTodos()
        {
            List<TodoItem> todoList = new();
            string connStr = "server=localhost;userid=todouser;password=todouser2022;database=TodoListDB";

            using (MySqlConnection connexion = new(connStr))
            {
                using (var tableCmd = connexion.CreateCommand())
                {
                    connexion.Open();
                    tableCmd.CommandText = "SELECT * FROM todo";
                    
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows) {
                            while (reader.Read())
                            {
                                todoList.Add(
                                    new TodoItem
                                    {
                                        Id = reader.GetInt32(0),
                                        Task = reader.GetString(1),
                                    });
                            }
                        }
                        else
                        {
                            return new TodoViewModel
                            {
                                TodoList = todoList
                            };
                        }
                       
                    };
                }                        
            }
            return new TodoViewModel 
            {
                TodoList = todoList 
            };
        }

        internal TodoItem GetById(int id)
        {
            TodoItem todo = new();
            string connStr = "server=localhost;userid=todouser;password=todouser2022;database=TodoListDB";

            using (MySqlConnection connexion = new(connStr))
            {
                using (var tableCmd = connexion.CreateCommand())
                {
                    connexion.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo WHERE Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            todo.Id = reader.GetInt32(0);
                            todo.Task = reader.GetString(1);
                        }
                        else
                        {
                            return todo;
                        }

                    };
                }
            }
            return todo;
        }

        public RedirectResult Insert(TodoItem todo)
        {
            // Chaine de connexion
            string connStr = "server=localhost;userid=todouser;password=todouser2022;database=TodoListDB";

            using (MySqlConnection connexion = new MySqlConnection(connStr))
            {
                using (var tableCmd = connexion.CreateCommand())
                {
                    connexion.Open();
                    tableCmd.CommandText = $"INSERT INTO todo (Task) VALUES ('{todo.Task}')";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return Redirect("https://localhost:7097/");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask([FromBody] TodoItem Id)
        {
            // Chaine de connexion
            string connStr = "server=localhost;userid=todouser;password=todouser2022;database=TodoListDB";

            using (MySqlConnection connexion = new MySqlConnection(connStr))
            {
                using (var tableCmd = connexion.CreateCommand())
                {
                    connexion.Open();
                    tableCmd.CommandText = $"DELETE FROM todo WHERE Id='{Id.Id}'";
                    Console.WriteLine(tableCmd.CommandText);
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return Json(new {});
            }
        }

        public RedirectResult Update(TodoItem todo)
        {
            // Chaine de connexion
            string connStr = "server=localhost;userid=todouser;password=todouser2022;database=TodoListDB";

            using (MySqlConnection connexion = new MySqlConnection(connStr))
            {
                using (var tableCmd = connexion.CreateCommand())
                {
                    connexion.Open();
                    tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Task}' WHERE Id = '{todo.Id}'";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return Redirect("https://localhost:7097/");
            }
        }
    }
}