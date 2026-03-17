using Microsoft.EntityFrameworkCore;
using ProjectSpace.Data;
using ProjectSpace.Dtos.Projects;
using ProjectSpace.Models;

namespace ProjectSpace.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        private static ProjectResponse ToResponse(Project p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status,
            Tasks = p.Tasks,
        };

        private static List<ProjectResponse> ToResponse(List<Project> projects) =>
            projects.Select(p => new ProjectResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                Tasks = p.Tasks
            }).ToList();


        public async Task<List<ProjectResponse>> GetAllProjectsAsync(string userId)
        {

            var projects = await _context.Projects
                            .Where(p => p.OwnerUserId == userId)
                            .OrderByDescending(p => p.CreatedAt)
                            .ToListAsync();

            return ToResponse(projects);
        }


        public async Task<List<ProjectResponse>> GetProjectByIdAsync(Guid projectId, string userId)
        {
            var project = await _context.Projects
                            .Where(p => p.OwnerUserId == userId && p.Id == projectId)
                            .ToListAsync();

            return ToResponse(project);
        }


        public async Task<ProjectResponse> AddProjectAsync(CreateProjectDto project, string userId)
        {
            var newProject = new Project
            {
                Id = Guid.NewGuid(),
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OwnerUserId = userId,
            };

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();

            return ToResponse(newProject);
        }


        public async Task<ProjectResponse> UpdateProjectAsync(Guid projectId, UpdateProjectDto dto, string userId)
        {
            var project = await _context.Projects
                            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerUserId == userId) ?? throw new Exception("Project not found or unauthorized");

            //if(project == null) 
            //{
            //    throw new Exception("Project not found");
            //}

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.Status = dto.Status;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ToResponse(project);
        }


        public async Task<bool> DeleteProjectAsync(Guid projectId, string userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerUserId == userId);

            if (project == null)
                return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
