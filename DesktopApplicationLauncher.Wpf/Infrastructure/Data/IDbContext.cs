namespace DesktopApplicationLauncher.Wpf.Infrastructure.Data
{
    using System;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Entities;

    public interface IDbContext : IDisposable
    {
        IDbCollection<Application> Applications { get; }
    }
}
