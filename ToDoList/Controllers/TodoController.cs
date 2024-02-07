using Microsoft.AspNetCore.Mvc;
using ToDoList.DAL.Abstract;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TodoController : Controller
    {
        readonly ITodoDAL _todoDAL;

        public TodoController(ITodoDAL todoDAL)
        {
            _todoDAL = todoDAL;
        }

        public IActionResult Index()
        {
            return Json(_todoDAL.GetUsersTodos(new Guid()));
        }

        [HttpPost]
        public IActionResult AddTodo(Todo todo)
        {
            return Json(_todoDAL.GetTodoById(new Guid(_todoDAL.AddTodo(todo))));
        }

        [HttpPost]
        public IActionResult ToggleTodo(string Id)
        {
            return _todoDAL.ToggleTodo(new Guid(Id)) ? StatusCode(200) : StatusCode(400);
        }

        [HttpPost]
        public IActionResult DeleteTodo(string Id)
        {
            return _todoDAL.DeleteTodo(new Guid(Id)) ? StatusCode(200) : StatusCode(400);
        }

        public IActionResult UpdateTodo(Todo todo)
        {
            return _todoDAL.UpdateTodo(todo) ? StatusCode(200) : StatusCode(400);
        }


    }
}
