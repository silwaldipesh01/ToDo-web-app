using Microsoft.AspNetCore.Mvc;
using ToDo_App.Model;
using ToDo_App.Model.DTO;
using ToDo_App.Repository;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToDoController :ControllerBase
    {
        [HttpGet]
      
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ToDoTask))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ToDoTaskDTO>> GetTasks() {

            var tds = new List<ToDoTaskDTO>();

            foreach (var todos in ToDoRepository.Takses)
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

            return Ok(tds);
        }

        [HttpGet("{id}")]
        public ActionResult<ToDoTaskDTO> GetTaskById(int id)
        {
            // Implementation to fetch a single task
            return Ok(new ToDoTaskDTO());
        }

        [HttpGet("{title:alpha}")]
        public ActionResult GetTaskByTitle(string title)
        {
            var task = ToDoRepository.Takses.FirstOrDefault(t => t.TaskTitle == title);
            if (task == null) return NotFound();

            return Ok(task);
        }


        [HttpPost("{title}",Name ="Dipesh")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddTasks(string title, [FromBody] ToDoTaskDTO? todos)
        {
            if (todos == null) return BadRequest("Empty Body");
            if (!TimeOnly.TryParse(todos.DueTime, out TimeOnly validatedTime))
            {
                return BadRequest("Invalid Time format. Please use HH:mm or HH:mm:ss");
            }
            ToDoTask T = new ToDoTask()
            {
                TaskTitle = todos.TaskTitle,
                TaskDescription = todos.TaskDescription,
                TaskDueDate = todos.TaskDueDate,
                DueTime =validatedTime.ToString(),
                TaskIsCompleted = todos.TaskIsCompleted,
                TaskPriority = todos.TaskPriority,
            };
            ToDoRepository.Takses.Add(T);

            return CreatedAtAction(nameof(GetTaskByTitle), new { title = todos.TaskTitle }, todos);

        }

        [HttpDelete("{title}")]
        [ProducesResponseType(StatusCodes.Status204NoContent,Type = typeof(ToDoTaskDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteByTitle(string title){
            var taskToDelete = ToDoRepository.Takses.FirstOrDefault(t => t.TaskTitle?.ToLower() == title.ToLower());
            if (taskToDelete == null)
            {
                return NotFound($"No task found with the title: {title}");
            }
            ToDoRepository.Takses.Remove(taskToDelete);

            return NoContent();

        }

        [HttpPut("{title}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ToDoTaskDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateTasks(string title, [FromBody] ToDoTaskDTO todos)
        {
            
            var existingTask = ToDoRepository.Takses.FirstOrDefault(t => t.TaskTitle == title);

            if (!TimeOnly.TryParse(todos.DueTime, out TimeOnly validatedTime))
            {
                return BadRequest("Invalid Time format. Please use HH:mm or HH:mm:ss");
            }

            if (existingTask == null) return NotFound("Task not found");

           
            existingTask.TaskTitle = todos.TaskTitle;
            existingTask.TaskDescription = todos.TaskDescription;
            existingTask.TaskIsCompleted = todos.TaskIsCompleted;
            existingTask.TaskDueDate = todos.TaskDueDate;
            existingTask.TaskPriority = todos.TaskPriority;
            existingTask.DueTime = validatedTime.ToString();



         
            return NoContent();
        }


    }
}
