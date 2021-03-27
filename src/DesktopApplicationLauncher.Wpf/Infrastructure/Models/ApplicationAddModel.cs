namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    public sealed class ApplicationAddModel
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Arguments { get; set; }

        public int SortOrder { get; set; }
    }
}
