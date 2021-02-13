namespace DesktopApplicationLauncher.Wpf
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Business;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Data;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Reviewed. LiteDbContext is disposed at OnExit method.")]
    public partial class App : Application
    {
        private IDbContext _liteDbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            _liteDbContext = new LiteDbContext("DesktopApplicationLauncher.db");

            var applicationService = new ApplicationService(_liteDbContext);
            applicationService.ListAllApplications();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _liteDbContext.Dispose();

            base.OnExit(e);
        }
    }
}
