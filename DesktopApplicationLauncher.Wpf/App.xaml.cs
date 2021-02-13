namespace DesktopApplicationLauncher.Wpf
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Business;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Data;
    using DesktopApplicationLauncher.Wpf.ViewModels;
    using DesktopApplicationLauncher.Wpf.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Reviewed. LiteDbContext is disposed at OnExit method.")]
    public partial class App
    {
        private IDbContext _liteDbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            _liteDbContext = new LiteDbContext("DesktopApplicationLauncher.db");

            ServiceLocator.Init(new ApplicationService(_liteDbContext));

            var mainWindow = new MainWindow { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            mainWindow.SourceInitialized += (s, a) => mainWindow.WindowState = WindowState.Maximized;
            mainWindow.DataContext = new MainViewModel(mainWindow);
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
