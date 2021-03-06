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
        {
            _database = new LiteDatabase(connectionString);

            Applications = new LiteDbCollection<Application>(_database);
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