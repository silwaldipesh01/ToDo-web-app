using ToDo_App.Model.DTO;

namespace ToDo_App.Services.Interfaces
{
    public interface IToDoAppServices
    {
        Task<ToDoTaskDTO> CreateToDoTask(ToDoTaskDTO taskDto);
        Task<bool> DeleteToDoTask(int id);
        List<ToDoTaskDTO> GetAllToDoTasks();
        ToDoTaskDTO GetToDoTaskById(int id);
        Task<ToDoTaskDTO> UpdateToDoTask(int id, ToDoTaskDTO taskDto);
    }
}