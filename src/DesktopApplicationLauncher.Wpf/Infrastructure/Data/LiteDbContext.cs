namespace DesktopApplicationLauncher.Wpf.Infrastructure.Data
{
    using System;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Entities;

    using LiteDB;

    public sealed class LiteDbContext : IDbContext
    {
        public IDbCollection<Application> Applications { get; }

        private readonly ILiteDatabase _database;
        private bool _disposed;

        public LiteDbContext(string connectionString)
            : this(new LiteDatabase(connectionString))
        {
        }

        public LiteDbContext(ILiteDatabase liteDatabase)
        {
            _database = liteDatabase ?? throw new ArgumentNullException(nameof(liteDatabase));

            Applications = new LiteDbCollection<Application>(_database);
            Applications.EnsureIndex(x => x.HierarchyPath);
        }

        ~LiteDbContext()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _database.Dispose();
                }

                _disposed = true;
            }
        }
    }
}