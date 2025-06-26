using DailyPlanner.Application.Interfaces.Repositories;
using DailyPlanner.Infrastructure.Persistence;
using DailyPlanner.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;

namespace DailyPlanner.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    // فایل appsettings.json را به عنوان منبع پیکربندی معرفی می‌کنیم
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // اضافه کردن DbContext به سرویس‌ها
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                        options.UseSqlServer(connectionString);
                    });

                    // معرفی Repository‌ها به DI Container
                    // هر جا IUserRepository درخواست شد، یک نمونه از UserRepository به آن بده
                    services.AddScoped<IUserRepository,  UserRepository>();
                    services.AddScoped<ITaskRepository, TaskRepository>();

                    // معرفی پنجره اصلی برنامه
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}