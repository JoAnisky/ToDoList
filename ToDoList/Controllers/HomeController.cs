using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using MySql.Data.MySqlClient;

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
            return View();
        }

        public void Insert(TodoVM todo)
        {
            string connStr = "server=localhost;userid=todouser;password=todouser2022;databse=TodoListDB";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (var tableCmd = conn.CreateCommand())
                {
                    conn.Open();
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
            }
        }    
    }
}