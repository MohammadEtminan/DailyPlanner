using System.Threading.Tasks;
using DailyPlanner.Domain.Entities;

namespace DailyPlanner.Application.Interfaces.Services
{
    /// <summary>
    /// اینترفیس برای سرویس احراز هویت
    /// </summary>
    public interface IAuthService
    {
        Task<User?> RegisterAsync(string username, string email, string password);
        Task<User?> LoginAsync(string email, string password);
        string GenerateJwtToken(User user);
    }
}