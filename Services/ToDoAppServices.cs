using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToDoAppServices(
            ToDoAppDbContext DbContext,
            IHubContext<TodoHub> hub,
            IHttpContextAccessor contextAccessor)
        {
            _todoAppDbContext = DbContext;
            _hub = hub;
            _httpContextAccessor = contextAccessor;
        }

        private int CurrentUserId => _httpContextAccessor.HttpContext?.User?.GetUserId() ?? 0;

        public List<ToDoTaskDTO> GetAllToDoTasks()
        {
            return _todoAppDbContext.ToDoTasks
                .Select(task => MapToDTO(task))
                .ToList();
        }

        public ToDoTaskDTO GetToDoTaskById(int id)
        {
            var task = _todoAppDbContext.ToDoTasks
                .FirstOrDefault(t => t.TaskId == id && t.UserId == CurrentUserId);

            return task == null ? null : MapToDTO(task);
        }

        public async Task<ToDoTaskDTO> CreateToDoTask(ToDoTaskDTO taskDto)
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
            await _todoAppDbContext.SaveChangesAsync();

            var dto = MapToDTO(newTask);

            await _hub.Clients.All.SendAsync("TaskCreated", dto);

            return dto;
        }

        public async Task<ToDoTaskDTO> UpdateToDoTask(int id, ToDoTaskDTO taskDto)
        {
            var existingTask = await _todoAppDbContext.ToDoTasks
                .FirstOrDefaultAsync(t => t.TaskId == id && t.UserId == CurrentUserId);

            if (existingTask == null)
                return null;

            existingTask.TaskTitle = taskDto.TaskTitle;
            existingTask.TaskDescription = taskDto.TaskDescription;
            existingTask.TaskDueDate = taskDto.TaskDueDate;
            existingTask.DueTime = taskDto.DueTime;
            existingTask.TaskIsCompleted = taskDto.TaskIsCompleted;
            existingTask.TaskPriority = taskDto.TaskPriority;

            await _todoAppDbContext.SaveChangesAsync();

            var dto = MapToDTO(existingTask);

            await _hub.Clients.All.SendAsync("TaskUpdated", dto);

            return dto;
        }

        public async Task<bool> DeleteToDoTask(int id)
        {
            var taskToDelete = await _todoAppDbContext.ToDoTasks
                .FirstOrDefaultAsync(t => t.TaskId == id && t.UserId == CurrentUserId);

            if (taskToDelete == null)
                return false;

            _todoAppDbContext.ToDoTasks.Remove(taskToDelete);
            await _todoAppDbContext.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("TaskDeleted", id);

            return true;
        }

        private static ToDoTaskDTO MapToDTO(ToDoTask task)
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