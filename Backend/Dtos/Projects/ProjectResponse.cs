using ProjectSpace.Enums;
using ProjectSpace.Models;

namespace ProjectSpace.Dtos.Projects
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ProjectStatus Status { get; set; }

        public List<TaskSummaryDto> Tasks { get; set; } = new();
        
    }
}
