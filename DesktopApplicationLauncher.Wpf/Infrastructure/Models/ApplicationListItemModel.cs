namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    using System;

    public sealed class ApplicationListItemModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Arguments { get; set; }

        public DateTime? LastAccessedDate { get; set; }
    }
}
