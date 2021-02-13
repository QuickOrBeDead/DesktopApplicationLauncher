namespace DesktopApplicationLauncher.Wpf.Infrastructure.Business
{
    using System;

    public static class ServiceLocator
    {
        private static IApplicationService s_ApplicationService;

        public static IApplicationService ApplicationService => s_ApplicationService;

        internal static void Init(IApplicationService applicationService)
        {
            s_ApplicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
        }
    }
}
