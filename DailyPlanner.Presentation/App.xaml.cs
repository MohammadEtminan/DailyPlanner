using Serilog;
using System.IO;
using System.Windows;
using Serilog.Sinks.MSSqlServer;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DailyPlanner.Presentation.Views;
using Microsoft.Extensions.Configuration;
using DailyPlanner.Infrastructure.Services;
using DailyPlanner.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using DailyPlanner.Application.Interfaces.Services;
using DailyPlanner.Application.Interfaces.Repositories;
using DailyPlanner.Infrastructure.Persistence.Repositories;
using DailyPlanner.Application.State;
using DailyPlanner.Infrastructure.State;

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
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    var connectionString = hostingContext.Configuration.GetConnectionString("DefaultConnection");

                    var sinkOptions = new MSSqlServerSinkOptions
                    {
                        TableName = "Logs", // نام جدولی که لاگ‌ها در آن ذخیره می‌شوند
                        AutoCreateSqlTable = true // اگر جدول وجود نداشت، به صورت خودکار بسازد
                    };

                    var columnOptions = new ColumnOptions();
                    // حذف ستون‌های اضافی که به آن‌ها نیاز نداریم
                    columnOptions.Store.Remove(StandardColumn.Properties);
                    columnOptions.Store.Add(StandardColumn.LogEvent); // این ستون تمام جزئیات را به صورت JSON ذخیره می‌کند

                    // تعریف ستون‌های سفارشی
                    columnOptions.AdditionalColumns = new List<SqlColumn>
                    {
                        new SqlColumn { ColumnName = "ActorId", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100, AllowNull = true }
                    };


                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext() // برای افزودن اطلاعات زمینه‌ای مثل ActorId
                        .WriteTo.MSSqlServer(
                            connectionString: connectionString,
                            sinkOptions: sinkOptions,
                            columnOptions: columnOptions
                         );
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
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<ITaskRepository, TaskRepository>();
                    services.AddScoped<IAuthService, AuthService>();
                    services.AddSingleton<ICurrentUserStore, CurrentUserStore>();

                    // ViewModel‌ها را به عنوان Singleton ثبت می‌کنیم تا حالت خود را حفظ کنند
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<LoginViewModel>();
                    services.AddSingleton<RegisterViewModel>();
                    services.AddSingleton<RegisterView>();
                    services.AddSingleton<DashboardViewModel>();

                    // معرفی پنجره اصلی برنامه
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();

            var registerView = (RegisterView)mainWindow.Content;
            registerView.DataContext = _host.Services.GetRequiredService<RegisterViewModel>();

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