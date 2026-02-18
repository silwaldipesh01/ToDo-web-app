using System.ComponentModel.DataAnnotations;

namespace ToDo_App.Model.DTO
{

    public class ToDoTaskDTO
    {
        [Required(ErrorMessage = "Task title is required.")]
        public string? TaskTitle { get; set; }
        public string? TaskDescription { get; set; }
        public bool TaskIsCompleted { get; set; }
        public DateOnly TaskDueDate { get; set; }
        public Priority TaskPriority { get; set; }

        public string DueTime { get; set; }

        
    }
}
