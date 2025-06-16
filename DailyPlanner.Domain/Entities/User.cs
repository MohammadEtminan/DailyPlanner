namespace DailyPlanner.Domain.Entities
{
    /// <summary>
    /// موجودیت کاربر که اطلاعات اصلی، نقش و روابط آن را نگهداری می‌کند
    /// </summary>
    public class User
    {
        #region Propeperties
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        // Nullable, چون کاربر می‌تواند با گوگل وارد شود
        public string? PasswordHash { get; set; }
        // Nullable, برای کاربرانی که با گوگل ثبت‌نام کرده‌اند
        public string? GoogleId { get; set; }
        public bool IsAdmin { get; set; } = false;
        #endregion

        #region Navigation Properties
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public NotificationSettings? NotificationSettings { get; set; }
        #endregion
    }
}