using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using MySql.Data.MySqlClient;
using ToDoList.Models.ViewModels;
using System.Data.Common;

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
            DBConnectInfos();
            TodoViewModel todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
           
        }

        public string DBConnectInfos()
        {
            try
            {
                DotNetEnv.Env.Load();
                string connStr = Environment.GetEnvironmentVariable("CONNEXION");

                if(connStr != null)
                {

                    return connStr;
                }

                return "";
            }
            catch(Exception error)
            {
                return "";
            }

        }

        public MySqlDataReader? DBQuery(string query)
        {
            try
            {
                string connStr = DBConnectInfos();

                if (!string.IsNullOrEmpty(connStr))
                {
                    using MySqlConnection connexion = new(connStr);

                    using MySqlCommand tableCmd = connexion.CreateCommand();

                    connexion.Open();
                    tableCmd.CommandText = query;//"SELECT * FROM todo";

                    using MySqlDataReader reader = tableCmd.ExecuteReader();
                    return reader;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult PopulateForm(int Id)
        {
            TodoItem todo = GetById(Id);
            return Json(todo);
        }

        internal TodoViewModel GetAllTodos()
        {
            List<TodoItem> todoList = new();
            string request = "SELECT Id, Task FROM todo";
            
            MySqlDataReader reader = DBQuery(request);
            if (reader != null)
            {
                if (reader.HasRows)
                {
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

                return new TodoViewModel
                {
                    TodoList = todoList
                };
            }

            return new TodoViewModel
            {
                TodoList = todoList
            };

        }

        internal TodoItem GetById(int id)
        {
            TodoItem todo = new();
            string connStr = DBConnectInfos();

            using (MySqlConnection connexion = new(connStr))
            {
                using (var tableCmd = connexion.CreateCommand())
                {
                    connexion.Open();
                    tableCmd.CommandText = $"SELECT Id, Task FROM todo WHERE Id = '{id}'";

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
            string connStr = DBConnectInfos();

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

        [HttpDelete]
        public JsonResult DeleteTask([FromBody] TodoItem Id)
        {
            // Chaine de connexion
            string connStr = DBConnectInfos();

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

        [HttpPatch]
        public RedirectResult Update(TodoItem todo)
        {
            // Chaine de connexion
            string connStr = DBConnectInfos();
            using (MySqlConnection connexion = new MySqlConnection(connStr))
            {
                using (var tableCmd = connexion.CreateCommand())
                {
                    connexion.Open();
                    tableCmd.CommandText = $"UPDATE todo SET Task = '{todo.Task}' WHERE Id = '{todo.Id}'";
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