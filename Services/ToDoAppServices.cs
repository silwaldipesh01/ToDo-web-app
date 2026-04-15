using Microsoft.AspNetCore.SignalR;
using ToDo_App.Data.Context;
using ToDo_App.Hubs;
using ToDo_App.Model;
using ToDo_App.Model.DTO;
using ToDo_App.Services.Interfaces;
using ToDo_App.Extensions;

namespace ToDo_App.Services
{
    public class ToDoAppServices : IToDoAppServices
    {
        private readonly ToDoAppDbContext _todoAppDbContext;
        private readonly IHubContext<TodoHub> _hub;
        //private readonly ILogger<ToDoAppServices> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ToDoAppServices(ToDoAppDbContext DbContext, IHubContext<TodoHub> hub, IHttpContextAccessor contextAccessor)
        {
            _todoAppDbContext = DbContext;
            _hub = hub;
           
            _httpContextAccessor = contextAccessor;
        }
        private int CurrentUserId => _httpContextAccessor.HttpContext?.User?.GetUserId() ?? 0;

        // Helper method to map Entity to DTO to avoid code repetition

        public List<ToDoTaskDTO> GetAllToDoTasks()
        {
            return _todoAppDbContext.ToDoTasks
             //   .Where(task => task.UserId == CurrentUserId)
                .Select(task => MapToDTO(task))
                .ToList();
        }

        public ToDoTaskDTO GetToDoTaskById(int id)
        {
            var task = _todoAppDbContext.ToDoTasks.FirstOrDefault(t => t.TaskId == id && t.UserId == CurrentUserId);
            return task == null ? null : MapToDTO(task);
        }

        public ToDoTaskDTO CreateToDoTask(ToDoTaskDTO taskDto)
        {
            var newTask = new ToDoTask
            {
                UserId = CurrentUserId,
                TaskTitle = taskDto.TaskTitle,
                TaskDescription = taskDto.TaskDescription,
                TaskDueDate = taskDto.TaskDueDate,
                DueTime = taskDto.DueTime,
                TaskIsCompleted = taskDto.TaskIsCompleted,
                TaskPriority = taskDto.TaskPriority,
            };

            _todoAppDbContext.ToDoTasks.Add(newTask);
            _todoAppDbContext.SaveChanges();

            return MapToDTO(newTask);
        }

        public ToDoTaskDTO UpdateToDoTask(int id, ToDoTaskDTO taskDto)
        {
            var existingTask = _todoAppDbContext.ToDoTasks.FirstOrDefault(t => t.TaskId == id);

            if (existingTask == null) return null;

            existingTask.TaskTitle = taskDto.TaskTitle;
            existingTask.TaskDescription = taskDto.TaskDescription;
            existingTask.TaskDueDate = taskDto.TaskDueDate;
            existingTask.DueTime = taskDto.DueTime;
            existingTask.TaskIsCompleted = taskDto.TaskIsCompleted;
            existingTask.TaskPriority = taskDto.TaskPriority;

            _todoAppDbContext.SaveChanges();

            return MapToDTO(existingTask);
        }

        public bool DeleteToDoTask(int id)
        {
            var taskToDelete = _todoAppDbContext.ToDoTasks.FirstOrDefault(t => t.TaskId == id);

            if (taskToDelete == null) return false;

            _todoAppDbContext.ToDoTasks.Remove(taskToDelete);
            _todoAppDbContext.SaveChanges();

            return true;
        }
        private ToDoTaskDTO MapToDTO(ToDoTask task)
        {
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
    }
}