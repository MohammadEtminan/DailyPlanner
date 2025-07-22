using System.Windows;
using DailyPlanner.Presentation.ViewModels;

namespace DailyPlanner.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // MainViewModel از طریق DI تزریق می‌شود
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // تنظیم DataContext اصلی پنجره
        }
    }
}