namespace DesktopApplicationLauncher.Wpf
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Business;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Data;
    using DesktopApplicationLauncher.Wpf.ViewModels;
    using DesktopApplicationLauncher.Wpf.Views;

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

            _liteDbContext = new LiteDbContext(configuration.GetConnectionString("AppDb"));

            var mainWindow = new MainWindow { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            mainWindow.SourceInitialized += (_, _) => mainWindow.WindowState = WindowState.Maximized;
            mainWindow.DataContext = new MainViewModel(mainWindow, new ApplicationService(_liteDbContext));
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _liteDbContext.Dispose();

            base.OnExit(e);
        }
    }
}
