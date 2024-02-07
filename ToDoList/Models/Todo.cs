using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Todo
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime? ExpiredOn { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsCompleted { get; set; }

    }
}
