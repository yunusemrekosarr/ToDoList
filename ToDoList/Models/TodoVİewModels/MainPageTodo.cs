using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.TodoVİewModels
{
    public class MainPageTodo
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public DateTime? ExpiredOn { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
