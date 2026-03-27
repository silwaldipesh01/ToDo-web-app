using ToDo_App.Data.Context;
using ToDo_App.Model;
using ToDo_App.Model.DTO;
using ToDo_App.Repository;
using ToDo_App.Services.Interfaces;

namespace ToDo_App.Services
{
    public class ToDoAppServices : IToDoAppServices
    {
        //Defing the dependency for the ToDoAppDbContext as prvate readonly field   
        private readonly ToDoAppDbContext _todoAppDbContext;

        public ToDoAppServices(ToDoAppDbContext DbContext)
        {
            _todoAppDbContext = DbContext;
        }

        public List<ToDoTaskDTO> GetAllToDoTasks()
        {
            var tds = new List<ToDoTaskDTO>();
            foreach (var todos in _todoAppDbContext.ToDoTasks)
            {
                ToDoTaskDTO T = new ToDoTaskDTO()
                {
                    TaskTitle = todos.TaskTitle,
                    TaskDescription = todos.TaskDescription,
                    TaskDueDate = todos.TaskDueDate,
                    DueTime = todos.DueTime,
                    TaskIsCompleted = todos.TaskIsCompleted,
                    TaskPriority = todos.TaskPriority,
                };
                tds.Add(T);
            }
            return tds;
        }
        public ToDoTaskDTO GetToDoTaskById(int id)
        {
            var task = _todoAppDbContext.ToDoTasks.FirstOrDefault(t => t.TaskId == id);
            if (task == null) return null;
            return new ToDoTaskDTO
            {
                TaskTitle = task.TaskTitle,
                TaskDescription = task.TaskDescription,
                TaskDueDate = task.TaskDueDate,
                DueTime = task.DueTime,
                TaskIsCompleted = task.TaskIsCompleted,
                TaskPriority = task.TaskPriority,
            };
        }
        public void CreateToDoTask(ToDoTaskDTO task)
        {
            var newTask = new ToDoTask
            {
                TaskTitle = task.TaskTitle,
                TaskDescription = task.TaskDescription,
                TaskDueDate = task.TaskDueDate,
                DueTime = task.DueTime,
                TaskIsCompleted = task.TaskIsCompleted,
                TaskPriority = task.TaskPriority,
            };
            _todoAppDbContext.ToDoTasks.Add(newTask);
            _todoAppDbContext.SaveChanges();
        }

        public void UpdateToDoTask(int id, ToDoTaskDTO task)
        {
            throw new NotImplementedException();
        }

        public void DeleteToDoTask(int id)
        {
            throw new NotImplementedException();
        }
    }
}
