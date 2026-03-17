using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectSpace.Dtos.Projects;
using ProjectSpace.Services.ProjectService;
using System.Security.Claims;

namespace ProjectSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] //All project endpoints require login
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectController(IProjectService service) 
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var projects = await _service.GetAllProjectsAsync(userId);

            return Ok(projects);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var projects = await _service.GetProjectByIdAsync(id, userId);

            return Ok(projects);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var project = await _service.AddProjectAsync(createProjectDto, userId);

            return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectDto updateProjectDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var project = await _service.UpdateProjectAsync(id, updateProjectDto, userId);

            return Ok(project);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProject(Guid id) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "User ID claim not found."
                });
            }

            var deleted = await _service.DeleteProjectAsync(id, userId);

            if (!deleted)
            {
                return NotFound(new { message = "Project not found." });
            }

            return Ok(new { message = "Project deleted successfully" });
        }
    }
}
