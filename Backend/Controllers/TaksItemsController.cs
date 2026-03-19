using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectSpace.Dtos.TaskItems;
using ProjectSpace.Services.TaskItemService;
using System.Security.Claims;

namespace ProjectSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _service;

        public TaskItemController(ITaskItemService service)
        {
            _service = service;
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var tasks = await _service.GetAllTasksAsync(userId);

            return Ok(tasks);
        }


        [HttpGet("project/{projectId:guid}")]
        public async Task<IActionResult> GetTasksByProjectId(Guid projectId)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var tasks = await _service.GetTasksByProjectIdAsync(projectId, userId);

            return Ok(tasks);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var task = await _service.GetTaskByIdAsync(id, userId);

            if (task == null)
            {
                return NotFound(new
                {
                    message = "Task not found."
                });
            }

            return Ok(task);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskItemsDto dto)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var task = await _service.AddTaskAsync(dto, userId);

            if (task == null)
            {
                return BadRequest(new
                {
                    message = "Project not found or unauthorized."
                });
            }

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskItemsDto dto)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var task = await _service.UpdateTaskAsync(id, dto, userId);

            if (task == null)
            {
                return NotFound(new
                {
                    message = "Task not found."
                });
            }

            return Ok(task);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var deleted = await _service.DeleteTaskAsync(id, userId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Task not found."
                });
            }

            return Ok(new
            {
                message = "Task deleted successfully."
            });
        }
    }
}