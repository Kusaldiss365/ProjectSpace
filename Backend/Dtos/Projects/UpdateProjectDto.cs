using ProjectSpace.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectSpace.Dtos.Projects
{
    public class UpdateProjectDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }
    }
}
