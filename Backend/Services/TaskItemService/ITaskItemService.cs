using ProjectSpace.Dtos.TaskItems;

namespace ProjectSpace.Services.TaskItemService
{
    public interface ITaskItemService
    {
        Task<List<TaskItemsResponseDto>> GetAllTasksAsync(string userId);
        Task<List<TaskItemsResponseDto>> GetTasksByProjectIdAsync(Guid projectId, string userId);
        Task<TaskItemsResponseDto?> GetTaskByIdAsync(Guid taskId, string userId);
        Task<TaskItemsResponseDto?> AddTaskAsync(CreateTaskItemsDto dto, string userId);
        Task<TaskItemsResponseDto?> UpdateTaskAsync(Guid taskId, UpdateTaskItemsDto dto, string userId);
        Task<bool> DeleteTaskAsync(Guid taskId, string userId);
    }
}
