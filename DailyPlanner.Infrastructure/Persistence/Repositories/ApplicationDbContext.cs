using DailyPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DailyPlanner.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// کلاس اصلی برای ارتباط با پایگاه داده با استفاده از EF Core
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // تعریف DbSetها برای هر کدام از موجودیت‌ها
        // اینها به EF Core می‌گویند که برای این مدل‌ها جدول بسازد
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<NotificationSettings> NotificationSettings { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- User Entity Configuration ---
            var userEntity = modelBuilder.Entity<User>();
            
            userEntity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            userEntity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            // اطمینان از اینکه ایمیل هر کاربر منحصر به فرد است
            userEntity.HasIndex(u => u.Email)
                .IsUnique();

            // تعریف رابطه یک-به-چند بین User و TaskItem
            userEntity.HasMany(u => u.Tasks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade); // اگر کاربر حذف شد، تسک‌هایش هم حذف شوند

            // تعریف رابطه یک-به-یک بین User و NotificationSettings
            userEntity.HasOne(u => u.NotificationSettings)
                .WithOne(ns => ns.User)
                .HasForeignKey<NotificationSettings>(ns => ns.UserId);

            // --- TaskItem Entity Configuration ---
            var taskItemEntity = modelBuilder.Entity<TaskItem>();

            taskItemEntity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            taskItemEntity.Property(t => t.Description)
                .HasMaxLength(1000); // توضیحات اختیاری است

            // --- NotificationSettings Entity Configuration ---
            var notificationSettingsEntity = modelBuilder.Entity<NotificationSettings>();

            notificationSettingsEntity.ToTable(tb => tb.HasCheckConstraint("CK_NotificationSettings_MinutesBeforeTask", "[MinutesBeforeTask] >= 0"));

            // --- Log Entity Configuration ---
            var logEntity = modelBuilder.Entity<Log>();

            logEntity.Property(l => l.Level)
                .IsRequired()
                .HasMaxLength(50);

            logEntity.Property(l => l.Message)
                .IsRequired();
        }
    }
}