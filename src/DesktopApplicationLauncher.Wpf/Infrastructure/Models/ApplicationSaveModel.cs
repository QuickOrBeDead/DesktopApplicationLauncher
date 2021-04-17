namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    using DesktopApplicationLauncher.Wpf.Infrastructure.Entities;

    public sealed class ApplicationSaveModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Arguments { get; set; }

        public ApplicationItemType ItemType { get; set; }
    }
}