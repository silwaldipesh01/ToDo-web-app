using ToDo_App.Data.Context;
using ToDo_App.Model.DTO;

namespace ToDo_App.Services.Interfaces
{
    public interface IToDoAppServices
    {
        List<ToDoTaskDTO> GetAllToDoTasks();
        ToDoTaskDTO GetToDoTaskById(int id);
        ToDoTaskDTO CreateToDoTask(ToDoTaskDTO task);
        ToDoTaskDTO UpdateToDoTask(int id, ToDoTaskDTO task);
        bool DeleteToDoTask(int id);
    }
}
