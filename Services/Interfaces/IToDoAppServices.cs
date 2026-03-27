using ToDo_App.Data.Context;
using ToDo_App.Model.DTO;

namespace ToDo_App.Services.Interfaces
{
    public interface IToDoAppServices
    {
        // Define service methods for ToDoApp 

        public List<ToDoTaskDTO> GetAllToDoTasks();
        public ToDoTaskDTO GetToDoTaskById(int id);
         
        public void CreateToDoTask(ToDoTaskDTO task);
        public void UpdateToDoTask(int id, ToDoTaskDTO task);
        public void DeleteToDoTask(int id);
    }
}
