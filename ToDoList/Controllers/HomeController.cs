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
            // Chaine de connexion
            string connStr = "server=localhost;userid=todouser;password=todouser2022;database=TodoListDB";

            // Instancie un objet MySqlConnection

/*            // Requête SQL
            var stm = "SELECT VERSION()";
            // Instancie un objet MySqlCommand (execute la requete)
            var cmd = new MySqlCommand(stm, connection);

            // Retourne la première ligne du résultat et surtout la première colonne
            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"MySQL Version : {version}");*/

            // Ferme la connexion

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
            }
        }    
    }
}