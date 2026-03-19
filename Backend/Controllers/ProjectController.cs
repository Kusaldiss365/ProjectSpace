using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectSpace.Dtos.Projects;
using ProjectSpace.Models;
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


        private string GetUserIdOrThrow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User ID claim not found.");

            return userId;
        }


        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userId = GetUserIdOrThrow();

            var projects = await _service.GetAllProjectsAsync(userId);

            return Ok(projects);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var userId = GetUserIdOrThrow();

            var project = await _service.GetProjectByIdAsync(id, userId);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Project not found."
                });
            }

            return Ok(project);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            var userId = GetUserIdOrThrow();

            var project = await _service.AddProjectAsync(createProjectDto, userId);

            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectDto updateProjectDto)
        {
            var userId = GetUserIdOrThrow();

            var project = await _service.UpdateProjectAsync(id, updateProjectDto, userId);

            if (project == null)
            {
                return NotFound(new
                {
                    message = "Project not found."
                });
            }

            return Ok(project);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProject(Guid id) 
        {
            var userId = GetUserIdOrThrow();

            var deleted = await _service.DeleteProjectAsync(id, userId);

            if (!deleted)
            {
                return NotFound(new { message = "Project not found." });
            }

            return Ok(new { message = "Project deleted successfully" });
        }
    }
}
