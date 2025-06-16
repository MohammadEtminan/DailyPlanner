using DailyPlanner.Domain.Entities;

namespace DailyPlanner.Application.Interfaces.Repositories
{
    /// <summary>
    /// اینترفیس برای عملیات مربوط به وظایف در دیتابیس
    /// </summary>
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId);
        Task<TaskItem> AddAsync(TaskItem task);
        Task<TaskItem> UpdateAsync(TaskItem task);
        Task<TaskItem> DeleteAsync(Guid id);    
    }
}