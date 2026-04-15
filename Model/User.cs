using System.ComponentModel.DataAnnotations;

namespace ToDo_App.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Store hashed passwords, not plain text
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // e.g., Admin, User

        public ICollection<ToDoTask>? ToDoTasks { get; set; }
    }
}