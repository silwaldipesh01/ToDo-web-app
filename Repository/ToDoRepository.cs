using ToDo_App.Model;

namespace ToDo_App.Repository
{
    public static class ToDoRepository
    {
        public static List<ToDoTask> Takses = new List<ToDoTask>()
        {
            new ToDoTask{
                TaskTitle = "Dot Net Class",
                TaskDescription = "Complete the assignment ",
                TaskDueDate =  new DateOnly(2026, 1, 4) ,
                DueTime ="15:11:00",
                TaskIsCompleted = false,
                TaskPriority = Priority.High},

            new ToDoTask{
                
                TaskTitle = "Wake Up",
                TaskDescription = "Wake Up Early  and go to washroom",
                TaskDueDate =new  DateOnly(2026, 1,4),
                DueTime ="15:11:00",
                TaskIsCompleted = false,
                TaskPriority = Priority.Low},


        };
    }
}
