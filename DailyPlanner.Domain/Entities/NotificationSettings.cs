namespace DailyPlanner.Domain.Entities
{
    /// <summary>
    /// موجودیت تنظیمات اعلان که تنظیمات ایمیل هر کاربر را ذخیره می‌کند
    /// </summary>
    public class NotificationSettings
    {
        public Guid Id { get; set; }
        public bool IsEmailNotificationEnabled { get; set; } = true;
        public int MinutesBeforeTask { get; set; } = 15;

        #region Foreign Key To User (One-To-One relationship
        public Guid UserId { get; set; }
        // مقدار پیش‌فرض 15 دقیقه
        public User User { get; set; } = null!;
        #endregion
    }
}