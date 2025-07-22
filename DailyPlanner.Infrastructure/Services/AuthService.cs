using Serilog.Context;
using Microsoft.Extensions.Logging;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Application.Interfaces.Services;
using DailyPlanner.Application.Interfaces.Repositories;

namespace DailyPlanner.Infrastructure.Services
{

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        
        public async Task<User?> RegisterAsync(string username, string email, string password)
        {
            // 1. بررسی اینکه آیا کاربری با این ایمیل وجود دارد یا خیر
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null) 
            {
                _logger.LogWarning("Registration failed: Email {Email} already exists.", email);
                return null;
            }

            // 2. هش کردن رمز عبور
            var passwordHash = PasswordHasher.Hash(password);

            // 3. ایجاد کاربر جدید
            var user = new User
            {
                Id = System.Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                NotificationSettings = new NotificationSettings()
            };

            // 4. ذخیره کاربر در دیتابیس
            await _userRepository.AddAsync(user);

            using (LogContext.PushProperty("ActorId", user.Id))
            {
                _logger.LogInformation("New  user registered successfully: {Username}", user.Username);
            }

            return user;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) 
            {
                _logger.LogWarning("Login failed: Invalid password for user {Email}.", email);
                return null;
            }

            using (LogContext.PushProperty("ActorId", user.Id))
            {
                _logger.LogInformation("User {Username} logged in successfully.", user.Username);
            }

            return user;
        }
        
        // این متد را هم در آینده برای توکن JWT پیاده‌سازی خواهیم کرد
        public string GenerateJwtToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}