using Microsoft.EntityFrameworkCore;
using ProjectSpace.Data;
using ProjectSpace.Dtos.TaskItems;
using ProjectSpace.Models;

namespace ProjectSpace.Services.TaskItemService
{
    public class TaskItemService : ITaskItemService
    {
        private readonly AppDbContext _context;

        public TaskItemService(AppDbContext context)
        {
            _context = context;
        }


        private static TaskItemsResponseDto ToResponse(TaskItem task) => new()
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt
        };


        public async Task<List<TaskItemsResponseDto>> GetAllTasksAsync(string userId)
        {
            var tasks = await _context.TaskItems
                .Include(t => t.Project)
                .Where(t => t.Project.OwnerUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return tasks.Select(ToResponse).ToList();
        }


        public async Task<List<TaskItemsResponseDto>> GetTasksByProjectIdAsync(Guid projectId, string userId)
        {
            var tasks = await _context.TaskItems
                .Include(t => t.Project)
                .Where(t => t.ProjectId == projectId && t.Project.OwnerUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return tasks.Select(ToResponse).ToList();
        }


        public async Task<TaskItemsResponseDto?> GetTaskByIdAsync(Guid taskId, string userId)
        {
            var task = await _context.TaskItems
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.OwnerUserId == userId);

            if (task == null)
            {
                return null;
            }

            return ToResponse(task);
        }


        public async Task<TaskItemsResponseDto?> AddTaskAsync(CreateTaskItemsDto dto, string userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == dto.ProjectId && p.OwnerUserId == userId);

            if (project == null)
            {
                return null;
            }

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                ProjectId = dto.ProjectId,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                DueDate = dto.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return ToResponse(task);
        }


        public async Task<TaskItemsResponseDto?> UpdateTaskAsync(Guid taskId, UpdateTaskItemsDto dto, string userId)
        {
            var task = await _context.TaskItems
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.OwnerUserId == userId);

            if (task == null)
            {
                return null;
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.DueDate = dto.DueDate;

            await _context.SaveChangesAsync();

            return ToResponse(task);
        }


        public async Task<bool> DeleteTaskAsync(Guid taskId, string userId)
        {
            var task = await _context.TaskItems
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.OwnerUserId == userId);

            if (task == null)
            {
                return false;
            }

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}