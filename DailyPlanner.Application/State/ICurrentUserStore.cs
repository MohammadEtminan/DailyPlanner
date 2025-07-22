using DailyPlanner.Domain.Entities;

namespace DailyPlanner.Application.State
{
    /// <summary>
    /// سرویسی برای نگهداری اطلاعات کاربر جاری که در سیستم لاگین کرده است
    /// </summary>
    public interface ICurrentUserStore
    {
        User? CurrentUser { get; set; }
        bool IsLoggedIn { get; }
    }
}