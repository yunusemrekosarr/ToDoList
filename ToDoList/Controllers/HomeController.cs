using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.DAL.Abstract;
using ToDoList.Models;
using ToDoList.Models.TodoVİewModels;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ITodoDAL _todoDAL;
        public HomeController(ILogger<HomeController> logger, ITodoDAL todoDAL)
        {
            _logger = logger;
            _todoDAL = todoDAL;
        }

        public IActionResult Index()
        {
            List<Todo> todos = _todoDAL.GetUsersTodos(new Guid("91587BEF-0EE6-4D93-B0F7-0DEB0DB6D72A"));

            return View(todos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}