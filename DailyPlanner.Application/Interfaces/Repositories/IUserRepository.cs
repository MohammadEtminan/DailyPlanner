using DailyPlanner.Domain.Entities;

namespace DailyPlanner.Application.Interfaces.Repositories
{
    // <summary>
    /// اینترفیس برای عملیات مربوط به کاربر در دیتابیس
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
    }
}