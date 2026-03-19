using ProjectSpace.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectSpace.Dtos.TaskItems
{
    public class UpdateTaskItemsDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        [Required]
        public TaskItemStatus Status { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
