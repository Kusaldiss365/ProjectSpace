using ProjectSpace.Dtos.Projects;

namespace ProjectSpace.Services.ProjectService
{
    public interface IProjectService
    {
        Task<List<ProjectResponse>> GetAllProjectsAsync(string userId);
        Task<List<ProjectResponse>> GetProjectByIdAsync(Guid projectId, string userId);
        Task<ProjectResponse> AddProjectAsync(CreateProjectDto project, string userId);
        Task<ProjectResponse> UpdateProjectAsync(Guid projectId, UpdateProjectDto project, string userId);
        Task<bool> DeleteProjectAsync(Guid projectId, string userId);
    }
}
