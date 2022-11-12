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
    }
}