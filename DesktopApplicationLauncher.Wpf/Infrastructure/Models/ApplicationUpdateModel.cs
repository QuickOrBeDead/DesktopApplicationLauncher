namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    public sealed class ApplicationUpdateModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Arguments { get; set; }
    }
}