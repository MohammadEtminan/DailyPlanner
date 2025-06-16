namespace DailyPlanner.Domain.Entities
{
    /// <summary>
    /// موجودیت وظیفه که جزئیات یک تسک را شامل می‌شود
    /// </summary>
    public class TaskItem
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        #region Foreign Key To User
        public Guid UserId { get; set; }
        // null! for EF Core relationship configuration
        public User User { get; set; } = null!;
        #endregion
    }
}