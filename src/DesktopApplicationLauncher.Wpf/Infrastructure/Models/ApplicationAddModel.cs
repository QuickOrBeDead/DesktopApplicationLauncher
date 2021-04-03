namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    using DesktopApplicationLauncher.Wpf.Infrastructure.Entities;

    public sealed class ApplicationAddModel
    {
        public int? ParentId { get; set; }

        public string Name { get; set; }

        public ApplicationItemType ItemType { get; set; }

        public string Path { get; set; }

        public string Arguments { get; set; }

        public int SortOrder { get; set; }
    }
}
