using System.ComponentModel.DataAnnotations;

namespace ToDo_App.Model
{
    public enum Priority
    {
        High,
        Medium,
        Low
    }
    public class ToDoTask
    {
        [Key]
        public int TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskDescription { get; set; }
        public bool TaskIsCompleted { get; set; }
        public DateOnly TaskDueDate { get; set; }
        public string DueTime { get; set; }
        public Priority TaskPriority { get; set; }

        public static int count = 0;

        public ToDoTask() { 
            TaskId = count++;
        }

    }
}