using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ToDo_App.Hubs;
using ToDo_App.Model.DTO;
using ToDo_App.Services.Interfaces;

namespace ToDo_App.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoAppServices _todoService;
        

        public ToDoController(IToDoAppServices todoService)
        {
            _todoService = todoService;
          
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<ToDoTaskDTO>> GetTasks()
        {
            var tasks = _todoService.GetAllToDoTasks();
            return Ok(tasks);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ToDoTaskDTO> GetTaskById(int id)
        {
            var task = _todoService.GetToDoTaskById(id);
            if (task == null) return NotFound($"Task with ID {id} not found.");

            return Ok(task);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ToDoTaskDTO> AddTasks([FromBody] ToDoTaskDTO? todoDto)
        {
            if (todoDto == null) return BadRequest("Task data is required.");

            if (!TimeOnly.TryParse(todoDto.DueTime, out _))
            {
                return BadRequest("Invalid Time format. Please use HH:mm or HH:mm:ss");
            }

            var createdTask = _todoService.CreateToDoTask(todoDto);

            // Returns 201 Created with the location of the new resource and the created DTO
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.TaskTitle }, createdTask);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ToDoTaskDTO> UpdateTasks(int id, [FromBody] ToDoTaskDTO todoDto)
        {
            if (!TimeOnly.TryParse(todoDto.DueTime, out _))
            {
                return BadRequest("Invalid Time format.");
            }

            var updatedTask = _todoService.UpdateToDoTask(id, todoDto);

            if (updatedTask == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            return Ok(updatedTask);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteTask(int id)
        {
            var wasDeleted = _todoService.DeleteToDoTask(id);

            if (!wasDeleted)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            return NoContent();
        }
    }
}