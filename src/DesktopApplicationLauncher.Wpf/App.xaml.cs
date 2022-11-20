namespace DesktopApplicationLauncher.Wpf
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Windows;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Business;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Data;
    using DesktopApplicationLauncher.Wpf.ViewModels;
    using DesktopApplicationLauncher.Wpf.Views;

    using LiteDB;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Interaction logic for App
    /// </summary>
    [SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Reviewed. LiteDbContext is disposed at OnExit method.")]
    public partial class App
    {
        private IDbContext _liteDbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
                .Build();

            BsonMapper.Global.EnumAsInteger = true;

            _liteDbContext = new LiteDbContext(configuration.GetConnectionString("AppDb"));

            var mainWindow = new MainWindow { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            mainWindow.SourceInitialized += (_, _) => mainWindow.WindowState = WindowState.Maximized;
            mainWindow.DataContext = new MainViewModel(mainWindow, new ApplicationService(_liteDbContext));
            mainWindow.Show();

            AppDomain.CurrentDomain.UnhandledException += (_, ev) =>
                HandleException(mainWindow, (Exception)ev.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (_, ev) =>
                {
                    HandleException(mainWindow, ev.Exception, "Application.Current.DispatcherUnhandledException");
                    ev.Handled = true;
                };

            TaskScheduler.UnobservedTaskException += (_, ev) =>
                {
                    HandleException(mainWindow, ev.Exception, "TaskScheduler.UnobservedTaskException");
                    ev.SetObserved();
                };

            base.OnStartup(e);
        }

        private static void HandleException(Window mainWindow, Exception ex, string source)
        {
            MessageBox.Show(mainWindow, ex.ToString(), $"Unhandled exception ({source})", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _liteDbContext.Dispose();

            base.OnExit(e);
        }
    }
}
