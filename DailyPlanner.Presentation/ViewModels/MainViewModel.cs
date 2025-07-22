using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyPlanner.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel اصلی برنامه که ناوبری بین صفحات را مدیریت می‌کند
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        // سرویس‌هایی برای ایجاد ViewModelهای دیگر
        private readonly LoginViewModel _loginViewModel;
        private readonly RegisterViewModel _registerViewModel;
        private readonly DashboardViewModel _dashboardViewModel;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        public MainViewModel(LoginViewModel loginViewModel, RegisterViewModel registerViewModel, DashboardViewModel dashboardViewModel)
        {
            _loginViewModel = loginViewModel;
            _registerViewModel = registerViewModel;
            _dashboardViewModel = dashboardViewModel;

            // صفحه پیش‌ فرض برنامه، صفحه لاگین است
            _currentViewModel = loginViewModel;
        }

        // کامندهایی برای تغییر صفحه
        // اینها را بعدا به LoginViewModel و RegisterViewModel اضافه می‌کنیم
        [RelayCommand]
        private void NavigateToLogin() => CurrentViewModel = _loginViewModel;

        [RelayCommand]
        private void NavigateToRegister() => CurrentViewModel = _registerViewModel;

        [RelayCommand]
        private void NavigateToDahboard() => CurrentViewModel = _dashboardViewModel;
    }
}