using System.Windows;
using CommunityToolkit.Mvvm.Input;
using DailyPlanner.Application.State;
using CommunityToolkit.Mvvm.ComponentModel;
using DailyPlanner.Application.Interfaces.Services;

namespace DailyPlanner.Presentation.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserStore _currentUserStore;
        private readonly MainViewModel _mainViewModel;

        // TODO: سرویس ناوبری و مدیریت کاربر جاری در مراحل بعد اضافه خواهد شد
        public LoginViewModel(IAuthService authService, ICurrentUserStore currentUserStore, MainViewModel mainViewModel)
        {
            _authService = authService;
            _currentUserStore = currentUserStore;
            _mainViewModel = mainViewModel;
        }

        [ObservableProperty]
        private string _email = string.Empty;

        // کامند لاگین که رمز عبور را به عنوان پارامتر دریافت می‌کند
        [RelayCommand]
        private async Task Login(string password)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Email and Password are required", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = await _authService.LoginAsync(Email, password);

            if (user != null) 
            {
                MessageBox.Show($"Welcome, {user.Username}!", "Successful Login", MessageBoxButton.OK, MessageBoxImage.Information);
                // TODO: ناوبری به صفحه داشبورد اصلی
            }
            else
            {
                MessageBox.Show("Email or Password is not valid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void GoToRegister()
        {
            // فراخوانی مستقیم کامند تولید شده در MainViewModel
            _mainViewModel.NavigateToRegisterCommand.Execute(null);
        }
    }
}