namespace DesktopApplicationLauncher.Wpf.Infrastructure.Entities
{
    using System;

    public sealed class Application
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public ApplicationItemType? ItemType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public string Arguments { get; set; }

        public int SortOrder { get; set; }

        public DateTime? LastAccessedDate { get; set; }

        public DateTime CreateDate { get; set; }

        public string HierarchyPath { get; set; }
    }
}
