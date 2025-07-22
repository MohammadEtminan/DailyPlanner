namespace DailyPlanner.Infrastructure.Services
{
    public class PasswordHasher
    {
        /// <summary>
        /// یک کلاس کمکی برای هش کردن و اعتبارسنجی رمزهای عبور با BCrypt
        /// </summary>
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}