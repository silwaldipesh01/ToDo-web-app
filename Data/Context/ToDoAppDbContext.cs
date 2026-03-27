using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using ToDo_App.Model;
namespace ToDo_App.Data.Context
{
    public class ToDoAppDbContext : DbContext
    {
        // Define a DbSet for ToDoTask entities
        public ToDoAppDbContext(DbContextOptions<ToDoAppDbContext> options) : base(options)
        {
        }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
    }
}
