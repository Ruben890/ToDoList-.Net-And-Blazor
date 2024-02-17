using Backend.Services.Tasks;
using Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/Tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TasksService _tasksService;
        public TasksController( TasksService tasksService) {
            _tasksService = tasksService;
        
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<TaskDTO>>> GetTasks()
        {
            var data = await _tasksService.GetTasks();
            return Ok(data);
        }

        



        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDTO>> GetTask(int id)
        {
            if (id == 0)
            {
                return BadRequest("no se ha proporsionado un Id");
            }

            var data = await _tasksService.GetTask(id);
            return Ok(data);
        }


        [HttpGet("Search")]
        [Authorize]
    public async Task<IActionResult> SearchTask([FromQuery] string title)
    {
        try
        {
            var tasks = await _tasksService.SearchTask(title);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ha ocurrido un error al buscar las tareas: " + ex.Message);
        }
    }


        [Authorize]
        [HttpPost("AddTask")]
        public async Task<ActionResult> AddTask(TaskDTO task)
        {
            await _tasksService.AddTask(task);
            return Ok( "Se ha agregado una nueva tarea");

        }

        [Authorize]
        [HttpPut("UpdateTask/{id}")]
        public async Task<ActionResult> UpdateTask(int id, TaskDTO task)
        {
            if(id == 0)
            {
                return BadRequest( "no se ha proporsionado un Id" );
            }

            await _tasksService.UpdateTask(id, task);

            return Ok( "Se ha actulizado la tarea" );
        }




        [Authorize]
        [HttpDelete("RemoveTask/{id}")]
        public async Task<ActionResult> RemoveTask(int id)
        {
            if (id == 0)
            {
                return BadRequest( "no se ha proporsionado un Id");
            }

            await _tasksService.DeleteTask(id);

            return Ok("tarea  borrada  exitosamente");
        }

    }
}
