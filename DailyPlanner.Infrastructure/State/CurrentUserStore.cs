using DailyPlanner.Domain.Entities;
using DailyPlanner.Application.State;

namespace DailyPlanner.Infrastructure.State
{
    public class CurrentUserStore : ICurrentUserStore
    {
        public User? CurrentUser { get; set; }

        public bool IsLoggedIn => CurrentUser != null;
    }
}