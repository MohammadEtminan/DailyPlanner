using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPlanner.Application.Interfaces.Services;
using System.Windows;

namespace DailyPlanner.Presentation.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        public RegisterViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        // این اتریبیوت‌ها به صورت خودکار پراپرتی‌های لازم برای بایندینگ در UI را می‌سازند
        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [RelayCommand]
        private async Task Register(string password)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(password))
            {
                System.Windows.MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // فراخوانی سرویس برای ثبت‌نام کاربر
            var newUser = await _authService.RegisterAsync(Username, Email, password);

            // نمایش پیام موفقیت یا خطا به کاربر
            if (newUser != null)
            {
                System.Windows.MessageBox.Show("Account created successfully! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // TODO: Navigate to login page
            }
            else
            {
                System.Windows.MessageBox.Show("An account with this email already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
