using Microsoft.EntityFrameworkCore;
using ToDo_App.Model;

namespace ToDo_App.Data.Context
{
    public class ToDoAppDbContext : DbContext
    {
        public ToDoAppDbContext(DbContextOptions<ToDoAppDbContext> options) : base(options)
        {
        }

        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
